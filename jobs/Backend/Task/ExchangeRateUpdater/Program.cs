using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbRates;
using ExchangeRateUpdater.Contracts;
using Refit;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly IReadOnlyCollection<Currency> Currencies = new[]
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

    public static async Task Main(string[] args)
    {
        try
        {
            var provider = new CnbExchangeRatesProvider(RestService.For<ICnbClient>("https://api.cnb.cz"));

            var rates = await provider.RetrieveExchangeRatesAsync(Currencies, CancellationToken.None);

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
    }
}