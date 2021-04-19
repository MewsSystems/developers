using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            //Not sure that we need a separate object for one string property, would rather use enum.
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
                var provider = new ExchangeRateProvider(new HttpClientWrapper());
                var results = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {results.Where(c => c.Success).Count()} exchange rates:");
               
                foreach (var result in results.Where(c => c.Success))
                {
                    Console.WriteLine(result.Rate.ToString());
                }

                foreach (var rate in results.Where(c => !c.Success))
                {
                    Console.WriteLine(rate.Message);
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
