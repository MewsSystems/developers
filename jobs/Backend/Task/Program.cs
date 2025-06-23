using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Ext;
using ExchangeRateUpdater.Model;

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
                var provider = new ExchangeRateProvider(new CnbClient());
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate);
                }

                foreach (var rate in rates)
                {
                    var exampleAmount = Math.Round(1000 * rate.Value, 2, MidpointRounding.AwayFromZero);
                    Console.WriteLine($"Converting 1000 {rate.SourceCurrency} to {rate.TargetCurrency} using ({rate}): {exampleAmount} {rate.TargetCurrency}");
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
