using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.DependencyResolution
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

            return services;
        }
    }
}
