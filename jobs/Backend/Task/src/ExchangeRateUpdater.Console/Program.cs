using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.BusinessLogic;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(s =>
    {
        s.AddLogging(l => l.AddConsole());
        s.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>();
        s.AddScoped<IExchangeRateManager>(sp =>
            new ExchangeRateManager(
                currencies => sp.GetRequiredService<IExchangeRateProvider>().GetExchangeRates(currencies)
            )
        );
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

try
{
    var manager = host.Services.GetRequiredService<IExchangeRateManager>();

    var currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    var rates = manager.GetExchangeRates(currencies);

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }

    return 0;
}
catch (Exception e)
{
    logger.LogError(e, "Could not retrieve exchange rates.");
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
    return 1;
}
finally
{
    Console.WriteLine("Press Enter to exit.");
    Console.ReadLine();
}