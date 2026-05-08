using System;
using System.IO;
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
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton(Console.Out);
        services.AddExhangeProviders(configuration);
        services.AddCache(configuration);
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

        var provider = configuration.GetValue<string>("CacheSettings:Provider") ?? "file";

        switch (provider.Trim().ToLowerInvariant())
        {
            case "redis":
                services.AddStackExchangeRedisCache(options =>
                {
                    var redisConfig = configuration.GetSection("CacheSettings:RedisConfiguration").Value;
                    options.Configuration = redisConfig;
                });
                services.AddSingleton<ICacheService, RedisCacheService>();
                break;

            case "file":
                var filePath = configuration.GetValue<string>("CacheSettings:FileCachePath") ?? "filecache.json";
                if (!Path.IsPathRooted(filePath))
                {
                    var baseDir = AppContext.BaseDirectory;
                    filePath = Path.GetFullPath(Path.Combine(baseDir, filePath));
                }
                services.AddSingleton<ICacheService>(new FileCacheService(filePath));
                break;
        }
    }
}
