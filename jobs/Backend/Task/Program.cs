using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static async Task Main(string[] args)
        {
            try
            {
                var factory = new ExchangeRateLoaderFactory();
                var loader = factory.Create();
                var parser = new ExchangeRateParser();
                var date = new DateProvider(new DateTimeService());
                var provider = new ExchangeRateProvider(loader, parser, date);
                var rates = await provider.GetExchangeRatesAsync(currencies);

                var enumerated = rates.ToList();
                Console.WriteLine($"Successfully retrieved {enumerated.Count} exchange rates:");
                foreach (var rate in enumerated)
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
