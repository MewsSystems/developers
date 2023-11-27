using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConfiguration(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var apiConfiguration = new ExchangeRateApiConfiguration();
        configuration.GetSection("ApiConfiguration").Bind(apiConfiguration);

        services.AddSingleton<IApiConfiguration>(apiConfiguration);
    }

    public static void AddExchangeRateUpdaterServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IExchangeRateHttpClient, ExchangeRateHttpClient>();
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
    }
}