using ExchangeRateUpdater.Service.Services;
using ExchangeRateUpdater.Service.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Service
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            return services;
        }
    }
}
