using ExchangeRateUpdater.ExchangeRateDataSources;
using ExchangeRateUpdater.ExchangeRateParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
            var httpClient = new HttpClient();

            try
            {
                var dataSource = new CnbExchangeRateDataSource(httpClient);
                var parser = new CnbExchangeRateParser();

                var provider = new ExchangeRateProvider(dataSource, parser);
                var rates = provider.GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();

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
