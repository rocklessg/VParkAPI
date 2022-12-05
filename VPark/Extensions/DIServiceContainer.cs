using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;
using VPark_Helper;

namespace VPark.Extensions
{
    public static class DIServiceContainer
    {
        public static void ResolveDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
            services.AddScoped<ILogger, Logger<ParkingSpaceRepository>>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IServiceFee, ServiceFee>();
            //services.AddScoped<IInterface, Implementation>();
        }
    }
}
