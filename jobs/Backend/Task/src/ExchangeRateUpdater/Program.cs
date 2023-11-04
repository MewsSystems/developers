using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly CancellationTokenSource Cts = new();

    private static readonly Currency[] Currencies = new[] {
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
        Console.CancelKeyPress += (_, _) => Cts.Cancel();

        var options = Options.Create(
            new ExchangeRateProviderOptions
            {
                CacheTtl = TimeSpan.FromMinutes(8)
            });
        
        using var httpClient = new HttpClient();
        var cnbClient = new CnbClient(httpClient, NullLogger<CnbClient>.Instance);

        try
        {
            var provider = new ExchangeRateProvider(options, cnbClient);
            var ratesResult = await provider.GetExchangeRates(Currencies, Cts.Token);
            ratesResult.Switch(
                rates =>
                {
                    Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                },
                error => Console.WriteLine(error.Message));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}