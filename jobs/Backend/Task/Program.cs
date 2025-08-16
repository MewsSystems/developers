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
                var provider = new ExchangeRateProvider();

                // The first implementation was for all currencies and the data is fetched from not Czech National Bank API
                // https://github.com/fawazahmed0/currency-api#readme
                // var rates = await provider.GetExchangeRatesAsync(currencies, DateTime.Now.Date);

                // The second implementation fetches data from Czech National Bank API
                // https://api.cnb.cz/cnbapi/swagger-ui.html#/
                var rates = await provider.GetCzechExchangeRateAsync(currencies, DateTime.Now.Date);

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
