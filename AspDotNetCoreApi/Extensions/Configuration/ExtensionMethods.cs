using AspDotNetCoreApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Extensions.Configuration
{
    public static class ExtensionMethods
    {
        // This is used to configure CORS
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowOrigins = configuration.GetSection("AllowOrigins")?.Get<string[]>();
            if(allowOrigins !=null && allowOrigins.Any())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins(allowOrigins)
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
                });
            }
            return services;
        }

        // Adds version information to the API
        public static IServiceCollection ConfigureVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(v =>
            {
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

        // Initiates the database connection string
        public static IServiceCollection ConfigureSetting(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new NullReferenceException("Configuration app settings failed");

            services.AddOptions();
            services.Configure<Settings>(options => options.ConnectionString = configuration["ConnectionString"] ??
            throw new ArgumentNullException($"Connection string error: " + 
            $"{nameof(options.ConnectionString)}"));

            return services;
        }

        // Consumes JWT attached in the request header.
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // options.SaveToken = true;
                    var secretSigningKey = configuration["IssuerSigningKey"] ??
                    throw new ArgumentException("Failed to found JWT secret issuer signing key");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["ValidIssuer"],
                        ValidAudience = configuration["ValidAudience"],

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretSigningKey))
                    };
                });

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services) => services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1.0", new OpenApiInfo
            {
                Version = "v1.0",
                Title = "ASP.NET API",
                Description = "URL to this ASP.NET Web API."
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
        });


    }
}
