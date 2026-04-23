using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Application.Services;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Api.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ExchangeRateProvider>();
        
        services.AddScoped<IExchangeRateProvider>(sp =>
        {
            var inner = sp.GetRequiredService<ExchangeRateProvider>();
            var cache = sp.GetRequiredService<IMemoryCache>();
            var logger = sp.GetRequiredService<ILogger<ExchangeRateProviderDecorator>>();
            var options = sp.GetRequiredService<IOptions<CnbApiOptions>>().Value;

            return new ExchangeRateProviderDecorator(
                inner, cache, logger,
                TimeSpan.FromMinutes(options.CacheDurationMinutes),
                TimeSpan.FromMinutes(options.HistoricalCacheDurationMinutes));
        });

        return services;
    }
}
