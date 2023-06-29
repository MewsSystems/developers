using System;
using System.Linq;
using System.Threading;
using ExchangeRateUpdater.CNBRateProvider;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddCnbRateProviderIntegration();

using IHost host = builder.Build();

Main(host.Services);

await host.RunAsync();

static void Main(IServiceProvider hostProvider)
{
    var czkCurrency = Currency.FromString("CZK");
    var currencies = new[]
    {
        new CurrencyPair(Currency.FromString("EUR"), czkCurrency),
        new CurrencyPair(Currency.FromString("JPY"), czkCurrency),
        new CurrencyPair(Currency.FromString("KES"), czkCurrency),
        new CurrencyPair(Currency.FromString("RUB"), czkCurrency),
        new CurrencyPair(Currency.FromString("THB"), czkCurrency),
        new CurrencyPair(Currency.FromString("TRY"), czkCurrency),
        new CurrencyPair(Currency.FromString("XYZ"), czkCurrency),
    };

    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var cts = new CancellationTokenSource();

    try
    {
        var exchangeRateProvider = provider.GetRequiredService<IExchangeRateProvider>();
        var ratesResult = exchangeRateProvider.GetExchangeRates(currencies, cts.Token).GetAwaiter().GetResult();

        if (ratesResult.IsFailed)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ratesResult.Errors}'.");
        }

        var rates = ratesResult.Value;

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
}

