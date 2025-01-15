using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
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
            })
            .AddStandardResilienceHandler();
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IMemoryCache, MemoryCache>();
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

while (true)
{
    try
    {
        var rates = await exchangeRateProvider.GetExchangeRates(currencies);

        if (!rates.Any())
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine("No exchange rates found for the chosen currencies.");
            Console.WriteLine(string.Empty);
            Console.WriteLine("Press any key to retry...");
            Console.ReadKey();
            Console.Clear();
            continue;
        }

        Console.WriteLine(string.Empty);
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        Console.WriteLine(string.Empty);

        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
            Console.WriteLine(string.Empty);
        }

        Console.WriteLine("Press any key to refresh...");
        Console.ReadKey();
        Console.Clear();
    }
    catch (Exception e)
    {
        Console.WriteLine(string.Empty);
        Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        Console.WriteLine(string.Empty);
        Console.WriteLine("Press any key to retry...");
        Console.ReadKey();
        Console.Clear();
    }
}
