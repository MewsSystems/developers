using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Installers;

public static class CacheInstaller
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration, bool useApiCache = true)
    {
        if (useApiCache)
        {
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));            
            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
            
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = cacheSettings.SizeLimit;
                options.CompactionPercentage = cacheSettings.CompactionPercentage;
                options.ExpirationScanFrequency = cacheSettings.ExpirationScanFrequency;
            });

            services.AddSingleton<IExchangeRateCache, ApiExchangeRateCache>();
        }
        else
        {
            services.AddSingleton<IExchangeRateCache, NoOpExchangeRateCache>();
        }

        return services;
    }
}