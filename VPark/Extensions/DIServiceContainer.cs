using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;

namespace VPark.Extensions
{
    public static class DIServiceContainer
    {
        public static void ResolveDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
            //services.AddScoped<IBooking, Booking>();
            //services.AddScoped<IPayment, Payment>();
        }
    }
}
