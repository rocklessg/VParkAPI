using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;

namespace VPark.Extensions
{
    public static class DIServiceContainer
    {
        public static void ResolveDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            //services.AddScoped<IInterface, Implementation>();
        }
    }
}
