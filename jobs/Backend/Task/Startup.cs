using System;
using ExchangeRateUpdater.Factories;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models.Cache;
using ExchangeRateUpdater.Models.Countries.CZE;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Cache;
using ExchangeRateUpdater.Services.Countries.CZE;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<App>();
        services.AddSingleton(Console.Out);
        services.AddExhangeProviders(configuration);
    }

    public static void AddExhangeProviders(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ExchangeRateProviderFactory>();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

        services.AddTransient<IExchangeRateProvider, CzeExchangeRateProvider>();
        services.AddHttpClient<CzeExchangeRateProvider>();
        services.Configure<CzeSettings>(configuration.GetSection("ExchangeProviders:CZE"));
    }

    public static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
        services.AddMemoryCache();
        services.AddStackExchangeRedisCache(options =>
        {
            var redisConfig = configuration.GetSection("CacheSettings:RedisConfiguration").Value;
            options.Configuration = redisConfig;
        });

        var provider = configuration.GetValue<string>("CacheSettings:Provider");

        if (string.Equals(provider, "Redis", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
    }
}
