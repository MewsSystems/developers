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

        services.AddSingleton(configuration.GetSection("ApiConfiguration").Get<ExchangeRateApiConfiguration>());
    }

    public static void AddExchangeRateUpdaterServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IExchangeRateHttpClient, ExchangeRateHttpClient>();
    }
}