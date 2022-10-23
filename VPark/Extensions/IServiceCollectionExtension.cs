using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
        }
}
