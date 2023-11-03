using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly IEnumerable<Currency> Currencies = new[]
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
        using var httpClient = new HttpClient();
        var cnbClient = new CnbClient(httpClient, NullLogger<CnbClient>.Instance);

        try
        {
            var provider = new ExchangeRateProvider(cnbClient);
            var rates = await provider.GetExchangeRates(Currencies);

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