using Microsoft.Extensions.Caching.Memory;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Installers;
using ExchangeRateUpdater.Api.Middleware;

namespace ExchangeRateUpdater.Api.Extensions;

public static class ExchangeRateApiInstaller
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApiServices();
        services.AddExchangeRateInfrastructure(configuration, useApiCache: true);

        return services;
    }

    //public static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddMemoryCache();
    //    services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
    //    var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
    //    services.Configure<MemoryCacheOptions>(options =>
    //    {
    //        options.SizeLimit = cacheSettings.SizeLimit;
    //        options.CompactionPercentage = cacheSettings.CompactionPercentage;
    //        options.ExpirationScanFrequency = cacheSettings.ExpirationScanFrequency;
    //    });

    //    return services;
    //}

    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), includeControllerXmlComments: true);
        });

        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
