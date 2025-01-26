using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Configurations;
using Infrastructure.Services;
using Infrastructure.Services.Data;
using Infrastructure.Services.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Registration;

public static class Registration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfigurations(configuration);
        services.AddCache();
        services.AddServices();
        services.AddData();
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CNBConfig>(configuration.GetSection("CNBSettings"));
    }

    private static void AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IHttpClientService, HttpClientService>();
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
    }

    private static void AddData(this IServiceCollection services)
    {
        services.AddSingleton<IAvailableCurrencies, AvailableCurrencies>();
        services.AddSingleton<IAvailableLangauges, AvailableLanguages>();
    }
}
