using ExchangeRateUpdater.Business.ExchangeRates;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.CNB;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.API.Configuration;

public static class DependenciesConfiguration
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        return services.AddApiLayer()
            .AddBusinessLayer()
            .AddInfrastructureLayer(configurationManager);
    }

    private static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }

    private static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<ExchangeRateProvider>();
        return services;
    }

    private static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddMemoryCache();
        services.AddScoped<ICnbProvider, CnbProvider>();
        services.AddScoped<ICacheProvider, CacheProvider>();
        services.Configure<CnbConfiguration>(configurationManager.GetSection("CnbConfiguration"));
        services.AddHttpClient<ICnbProvider, CnbProvider>((sp, httpClient) =>
        {
            IOptions<CnbConfiguration> cnbConfiguration = sp.GetRequiredService<IOptions<CnbConfiguration>>();
            httpClient.BaseAddress = new Uri(cnbConfiguration.Value.BaseAddress);
        });
        return services;
    }
}