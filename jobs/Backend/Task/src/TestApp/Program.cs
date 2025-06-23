using ExchangeRateProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestApp;

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddExchangeRateProviderServices(context);
    });

var host = builder.Build();


IEnumerable<Currency> currencies =
[
    new Currency("USD"),
    new Currency("EUR"),
    new Currency("CZK"),
    new Currency("JPY"),
    new Currency("KES"),
    new Currency("RUB"),
    new Currency("THB"),
    new Currency("TRY"),
    new Currency("XYZ")
];

try
{
    var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
    var rates = await provider.GetExchangeRatesAsync(currencies)
        .ConfigureAwait(false);

    var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
    Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");
    foreach (var rate in exchangeRates)
    {
        Console.WriteLine(rate.ToString());
    }
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.WriteLine("Press any key to close the app.");
Console.ReadKey();
