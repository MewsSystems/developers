using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastucture.Data.API.Abstractions;
using ExchangeRateUpdater.Infrastucture.Data.API.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddExchangeRateUpdaterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<Currency>();
            services.AddTransient<ExchangeRate>();
            services.AddTransient<IExternalAPIService, ExternalAPIService>();
            return services;
        }
    }
}
