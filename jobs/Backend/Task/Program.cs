using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
            RateProviderConfiguration rateProviderConfig = GetRateProviderConfiguration();
            try
            {
                var provider = new ExchangeRateProvider(rateProviderConfig);
                var rates = await provider.GetExchangeRatesAsync(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                if (e is TypeInitializationException && e.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {e.InnerException.Message}");
                    Console.WriteLine(e.InnerException.StackTrace);
                }
            }

            Console.ReadLine();
        }

        private static RateProviderConfiguration GetRateProviderConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var rateProviderConfig = new RateProviderConfiguration
            {
                Url = configuration["ApiConfiguration:Url"],
                BaseCurrency = configuration["ApiConfiguration:BaseCurrency"]
            };

            return rateProviderConfig;
        }
    }
}
