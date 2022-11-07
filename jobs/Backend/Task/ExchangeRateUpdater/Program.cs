using System;
using System.Linq;
using ExchangeRateUpdater.App;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddHttpClient<IExchangeRateClient, ExchangeRateClient>(client =>
        {
            client.BaseAddress =
                new Uri(
                    "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt");
        });

        services.AddScoped<ExchangeRateProvider>();
    }).Build();

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
    new Currency("XYZ"),
    new Currency("PHP")
};

try
{
    using var serviceScope = host.Services.CreateScope();
    var serviceProvider = serviceScope.ServiceProvider;

    var exchangeRateProvider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

    var rates = (await exchangeRateProvider.GetExchangeRates(currencies)).ToList();

    Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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

await host.RunAsync();
