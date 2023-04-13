using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.Options;

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
    services.Configure<CnbSettings>(configuration.GetSection(CnbSettings.SETTINGS_KEY));
    services.Configure<CurrenciesSettings>(configuration.GetSection(CurrenciesSettings.SETTINGS_KEY));

    // add services:
    services.AddSingleton<IClock, SystemClock>();
    services.AddHttpClient<IExchangeRateFetcher, CzechExchangeRateFetcher>();
    services.AddTransient<IExchangeRateParser, CzechExchangeRateParser>();
    services.AddTransient<IExchangeRateProvider, CzechExchangeRateProvider>();
}

// create service collection
var services = new ServiceCollection();
ConfigureServices(services);

// create service provider
using var serviceProvider = services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
var config = serviceProvider.GetRequiredService<IOptions<CurrenciesSettings>>();
var currencies = config.Value.Currencies.Select(code => new Currency(code));

try
{
    var rates = await provider.GetExchangeRates(currencies);

    logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        logger.LogInformation(rate.ToString());
    }
}
catch (InvalidOperationException e)
{
    logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
}
catch (Exception e)
{
    logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.ReadLine();
