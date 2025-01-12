using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>(
            client => 
            {
                client.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
            });
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddSingleton(TimeProvider.System);
        services.AddLogging();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
    })
    .Build();

var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();

IEnumerable<Currency> currencies = new[]
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

try
{
    var rates = exchangeRateProvider.GetExchangeRates(currencies).Result;

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.ReadLine();