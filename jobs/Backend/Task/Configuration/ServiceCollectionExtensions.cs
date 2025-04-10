using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExchangeRateUpdaterConfiguration>(configuration);
            services.AddMemoryCache();
            services.AddSingleton<ICustomHttpClient, CustomHttpClient>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ICurrencyProvider, CurrencyProvider>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

            return services;
        }
    }
}
