using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core.Configuration;

public static class DependencyResolver
{
    public static IServiceProvider Initialize()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        services.AddSingleton<IExchangeRateHttpClient, ExchangeRateHttpClient>();
        
        return services.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        services.AddSingleton(configuration.GetSection("ApiConfiguration").Get<ExchangeRateApiConfiguration>());
        services.AddHttpClient();
    }
}