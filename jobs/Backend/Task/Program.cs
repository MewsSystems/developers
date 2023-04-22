using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;

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
                var provider = new CnbExchangeRateProvider();
                provider.PrintExchangeRates(currencies);

                // Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");


            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
