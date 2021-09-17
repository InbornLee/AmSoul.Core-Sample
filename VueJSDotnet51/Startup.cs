using AmSoul.Core.DependencyInjection;
using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using AmSoul.Core.Services;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VueCliMiddleware;
using VueJSDotnet51.Models;
using VueJSDotnet51.Services;

namespace VueJSDotnet51
{
    public class Startup
    {
        public readonly IConfiguration Configuration;
        private readonly JwtTokenOptions tokenOptions;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            tokenOptions = configuration.GetSection(nameof(JwtTokenOptions)).Get<JwtTokenOptions>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperConfig));

            services.AddMongoDb(Configuration);
            services.AddIdentityMongoDBProvider<BaseUser, BaseRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            },
            database =>
            {
                var dbOptions = Configuration.GetSection(nameof(MongoDbDatabaseSettings)).Get<MongoDbDatabaseSettings>();
                database.Host = dbOptions.Host;
                database.Port = dbOptions.Port;
                database.Username = dbOptions.Username;
                database.Password = dbOptions.Password;
                database.DatabaseName = dbOptions.DatabaseName;
                database.SslSettings = dbOptions.SslSettings;
                database.ClusterConfigurator = dbOptions.ClusterConfigurator;
            }
            );

            services.AddControllers();
   
            services.AddJwtAuthentication(Configuration, tokenOptions);
            services.AddJwtSwaggerGen("TestServer", "v1");

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BookService>().As<IAsyncRESTService<Book>>();
            builder.RegisterType<TeamService>().As<IAsyncRESTService<Team>>();
            builder.RegisterType<UserServiceBase>().As<IUserService>();
            builder.RegisterType<GridFSServiceBase>().As<IGridFSService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestServer v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "ClientApp/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }
            });
        }
    }
}
