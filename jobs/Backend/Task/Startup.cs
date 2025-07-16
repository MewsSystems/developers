using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class Startup
{
    /// <summary>
    /// Configures application services, logging, HTTP client and dependency injection.
    /// Reads settings from appsettings.json.
    /// </summary>
    public static ServiceProvider ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection(); 
        
        // TODO: extract config + DI to use `IHostBuilder` if migrating to ASP.NET Core or Worker Service.
        
        services.AddSingleton<IConfiguration>(configuration);

        services.AddOptions<CnbOptions>()
                .Bind(configuration.GetSection("Cnb"));

        services.AddLogging(builder =>
        {
            var loggingSection = configuration.GetSection("Logging");
            builder.AddConfiguration(loggingSection);
            if (loggingSection.GetValue<bool>("EnableConsole"))
                builder.AddConsole();
        });

        services.AddHttpClient();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddTransient<ICnbXmlParser, CnbXmlParser>();

        return services.BuildServiceProvider();
    }
}
