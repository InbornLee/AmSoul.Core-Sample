using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AmSoul.Core.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbDatabaseSettings>(configuration.GetSection(nameof(MongoDbDatabaseSettings)));
            services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MongoDbDatabaseSettings>>().Value);

        }
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, JwtTokenOptions tokenOptions)
        {
            services.Configure<JwtTokenOptions>(configuration.GetSection(nameof(JwtTokenOptions)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtTokenOptions>>().Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptions.Issuer,

                        ValidateAudience = true,//验证Audience
                        ValidAudience = tokenOptions.Audience,

                        RequireExpirationTime = true,
                        ValidateLifetime = true,//验证失效时间

                        ValidateIssuerSigningKey = true, //验证SecurityKey
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)), //将密钥添加到JWT加密算法中
                    };
                });
        }
        public static void AddJwtSwaggerGen(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "输入 JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, System.Array.Empty<string>() }
                });

            });

        }
    }
}
