using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

static void ConfigureServices(IServiceCollection services)
{
    // configure logging
    services.AddLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    });

    // build config
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    services.Configure<CNBSettings>(configuration.GetSection(CNBSettings.SettingsKey));

    // add services:
    services.AddHttpClient<IExchangeRateFetcher, ExchangeRateFetcher>();
    services.AddSingleton<IExchangeRateParser, ExchangeRateParser>();
    services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
    services.AddSingleton<IClock, SystemClock>();
}

// create service collection
var services = new ServiceCollection();
ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
var config = serviceProvider.GetRequiredService<IConfiguration>();
var currencyCodes = config.GetSection("Currencies").Get<List<string>>();
var currencies = currencyCodes.Select(code => new Currency(code));

try
{
    var rates = await provider.GetExchangeRates(currencies);

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        logger.LogInformation(rate.ToString());
    }
}
catch (Exception e)
{
    logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.ReadLine();
