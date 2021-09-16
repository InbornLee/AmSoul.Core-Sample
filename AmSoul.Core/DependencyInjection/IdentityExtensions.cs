using AmSoul.Core.Converters;
using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using AmSoul.Core.Stores;
using AmSoul.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AmSoul.Core.DependencyInjection
{
    public static class IdentityExtensions
    {
        public static IMongoDatabase GetDatabase(IDatabaseSettings settings) => new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName ?? "default");

        public static IMongoCollection<T> GetCollection<T>(IDatabaseSettings settings, string collectionName)
        {
            IMongoCollection<T> collection;

            return collection = GetDatabase(settings)
                .GetCollection<T>(collectionName ?? nameof(T).ToLowerInvariant());
        }

        private static FindOptions<TItem> LimitOneOption<TItem>() => new() { Limit = 1 };

        public static IdentityBuilder AddIdentityMongoDBProvider<TUser, TRole>(this IServiceCollection services,
            Action<IdentityOptions> setupIdentityAction, Action<MongoDbDatabaseSettings> setupDatabaseAction)
            where TUser : BaseUser
            where TRole : BaseRole
        {
            return AddIdentityMongoDBProvider<TUser, TRole, ObjectId>(services, setupIdentityAction, setupDatabaseAction);
        }
        public static IdentityBuilder AddIdentityMongoDBProvider<TUser, TRole, TKey>(this IServiceCollection services,
            Action<IdentityOptions> identitySettingsAction, Action<MongoDbDatabaseSettings> databaseSettingsAction, IdentityErrorDescriber identityErrorDescriber = null)
            where TUser : BaseUser<TKey>
            where TRole : Role<TKey>
            where TKey : IEquatable<TKey>
        {
            var dbOptions = new MongoDbDatabaseSettings();
            databaseSettingsAction(dbOptions);

            //var migrationCollection = GetCollection<MigrationHistory>(dbOptions, dbOptions.MigrationCollection);
            var userCollection = GetCollection<TUser>(dbOptions, dbOptions.UsersCollection);
            var roleCollection = GetCollection<TRole>(dbOptions, dbOptions.RolesCollection);

            //Migrator.Apply<TUser, TRole, TKey>(migrationCollection, userCollection, roleCollection);

            var builder = services.AddIdentity<TUser, TRole>(identitySettingsAction ?? (s => { }));

            builder.AddRoleStore<RoleStore<TRole, TKey>>()
                .AddUserStore<UserStore<TUser, TRole, TKey>>()
                .AddUserManager<UserManager<TUser>>()
                .AddRoleManager<RoleManager<TRole>>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.AddSingleton(x => userCollection);
            services.AddSingleton(x => roleCollection);
            if (typeof(TKey) == typeof(ObjectId))
            {
                TypeConverterResolver.RegisterTypeConverter<ObjectId, ObjectIdConverter>();
            }

            // Identity Services
            services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole, TKey>(roleCollection, identityErrorDescriber));
            services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole, TKey>(userCollection, roleCollection, identityErrorDescriber));

            return builder;

        }
        public static async Task<TItem> FirstOrDefaultAsync<TItem>(this IMongoCollection<TItem> collection, Expression<Func<TItem, bool>> p, CancellationToken cancellationToken = default) => collection != null
        ? await (await collection.FindAsync(p, LimitOneOption<TItem>(), cancellationToken).ConfigureAwait(false)).FirstOrDefaultAsync().ConfigureAwait(false)
        : throw new ArgumentNullException(nameof(collection));

        public static async Task<IEnumerable<TItem>> WhereAsync<TItem>(this IMongoCollection<TItem> collection, Expression<Func<TItem, bool>> p, CancellationToken cancellationToken = default) => collection != null
        ? (await collection.FindAsync(p, cancellationToken: cancellationToken).ConfigureAwait(false)).ToEnumerable()
        : throw new ArgumentNullException(nameof(collection));

        public static string GetIdentityErrors(IdentityResult result)
        {
            string errors = "";
            foreach (var error in result.Errors)
            {
                errors += error.Description;
            }
            return errors;
        }
    }
}
