using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.CnbProvider;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
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

        public static void Main(string[] args)
        {
            try
            {
                var provider = new ExchangeRateProvider(new CnbRateProvider(new CnbRateProviderClient())); ;
                var rates = provider.GetExchangeRatesAsync(currencies, DateTime.UtcNow).Result;

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
}
