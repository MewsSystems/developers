using ExchangeRateUpdater.Abstractions.Interfaces;
using ExchangeRateUpdater.Abstractions.Model;
using ExchangeRateUpdater.CnbClient.Implementation;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Api.DependencyInjection;

/// <summary>
/// Dependency injection extensions for the Exchange Rate Updater API.
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Adds services for exchange rate updater API.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddExchangeRateUpdaterServices(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddScoped<IExchangeRatesService, ExchangeRateService>();
        services.AddSingleton<ICache<CurrencyValue>, InMemoryExchangeRateCache>();
        services.AddScoped<IExchangeRatesClientStrategy>(sp =>
        {
            var innerClient = new HttpExchangeRatesClientStrategy(sp.GetRequiredService<IHttpClientFactory>(), sp.GetRequiredService<IConfiguration>());
            var cache = sp.GetRequiredService<ICache<CurrencyValue>>();
            var logger = sp.GetRequiredService<ILogger<HttpExchangeRatesClientWithCacheStrategy>>();
            return new HttpExchangeRatesClientWithCacheStrategy(innerClient, cache, logger);
        });
        
        return services;
    }
}