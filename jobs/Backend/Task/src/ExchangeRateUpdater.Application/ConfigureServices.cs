using ExchangeRateUpdater.Application.Clients;
using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.ExchangeRates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddHttpClient();
        services.AddMemoryCache();
        services.AddScoped<IExchangeRateProvider,ExchangeRateProvider>();
        services.AddScoped<ICzbExchangeRateClient, CzbExchangeRateClient>();
        services.Configure<CzbOptions>(config.GetSection("CZB"));

        return services;
    }
}