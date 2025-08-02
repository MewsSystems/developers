using System;
using ExchangeRateUpdater.Factories;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Countries.CZE;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<App>();
        services.AddSingleton(Console.Out);
        services.AddExhangeProviders();
    }

    public static void AddExhangeProviders(this IServiceCollection services)
    {
        services.AddTransient<ExchangeRateProviderFactory>();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

        services.AddTransient<IExchangeRateProvider, CzeExchangeRateProvider>();
        services.AddHttpClient<CzeExchangeRateProvider>();
    }
}
