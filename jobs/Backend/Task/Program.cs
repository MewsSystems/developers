using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater;

public static class Program
{
    private static HashSet<Currency> currencies =
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

    public static async Task Main(string[] args)
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var exchangeRatesUrl = configuration["ExchangeRatesUrl"];
            if (string.IsNullOrWhiteSpace(exchangeRatesUrl))
            {
                Console.WriteLine("ExchangeRatesUrl has not been provided");
                return;
            }
            
            var provider = new ExchangeRateProvider(exchangeRatesUrl);
            var rates = (await provider.GetExchangeRates(currencies)).ToArray();

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
}