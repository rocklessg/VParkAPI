using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;

namespace VPark.Extensions
{
    public static class DIServiceContainer
    {
        public static void ResolveDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
            services.AddScoped<ILogger, Logger<ParkingSpaceRepository>>();
            services.AddScoped<IAccountRepository, AccountRepository>();   
           
            //services.AddScoped<IInterface, Implementation>();
        }
    }
}
