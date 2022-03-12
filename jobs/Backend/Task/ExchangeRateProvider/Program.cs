using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExchangeRateProvider
{
    class Program
    {
        private const string ratesUrl = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private static IEnumerable<CurrencyCode> currencies = new[]
        {
            new CurrencyCode("USD"),
            new CurrencyCode("EUR"),
            new CurrencyCode("CZK"),
            new CurrencyCode("JPY"),
            new CurrencyCode("KES"),
            new CurrencyCode("RUB"),
            new CurrencyCode("THB"),
            new CurrencyCode("TRY"),
            new CurrencyCode("XYZ")
        };

        static void Main(string[] args)
        {
            string cachedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "exchangeRates.csv");

            try
            {
                var provider = new ExchangeRateProvider(new ExchangeRateFileHandler { CachedFilePath = cachedFilePath });
                var rates = provider.GetExchangeRates(currencies, ratesUrl);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                Console.WriteLine($"{e.StackTrace}");
            }

            Console.ReadLine();
        }
    }
}
