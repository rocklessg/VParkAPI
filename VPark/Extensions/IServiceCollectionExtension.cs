using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using VPark_Models.Models;
using static System.Net.WebRequestMethods;

namespace VPark.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void ResolveSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Car Parking Service API",
                    Version = "v1",
                    Description = @"Software Application for managing parking lots",
                    Contact = new OpenApiContact
                    {
                        Name = "Abdulhafiz & Charles",
                        Email = "support@vparkers.com"
                    }
                });
            });
        }

        public static void ResolveJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            //var key = Environment.GetEnvironmentVariable("KEY");
            var key = jwtSettings.GetSection("Key").Value;

            _ = services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }

        public static void ResolvePayStack(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("Paystack");
            services.Configure<PaystackSettings>(configuration);
        }
    }
}
