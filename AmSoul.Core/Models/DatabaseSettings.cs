using AmSoul.Core.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;

namespace AmSoul.Core.Models
{
    public class MongoDbDatabaseSettings : IDatabaseSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string DatabaseName { get; set; }
        public SslSettings SslSettings { get; set; }
        public Action<ClusterBuilder> ClusterConfigurator { get; set; }
        public string UsersCollection { get; set; } = "users";

        public string RolesCollection { get; set; } = "roles";

        public string MigrationCollection { get; set; } = "_migrations";

        public string ConnectionString
        {
            get
            {
                return string.IsNullOrEmpty(Username) ? $"mongodb://{Host}:{Port}" : $"mongodb://{Username}:{Password}@{Host}:{Port}";
            }
        }
    }
    public class MySqlDatabaseSettings : IDatabaseSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string DatabaseName { get; set; }
        public string ConnectionString
        {
            get
            {
                return $"Server={Host};Port=3306;Database={DatabaseName};Uid={Username};Pwd={Password}";
            }
        }
    }

}
