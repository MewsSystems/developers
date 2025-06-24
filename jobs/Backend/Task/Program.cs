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
            
            try
            {
                var provider = new ExchangeRateProvider(GetRateProviderConfiguration());
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

        private static IExchangeRateProviderConfiguration GetRateProviderConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var rateProviderConfig = new ExchangeRateProviderConfiguration
            {
                Url = configuration["ApiConfiguration:Url"],
                BaseCurrency = configuration["ApiConfiguration:BaseCurrency"]
            };

            if (string.IsNullOrWhiteSpace(rateProviderConfig.Url))
                throw new Exception("ApiConfiguration:Url is not set in appsettings.json");

            if (string.IsNullOrWhiteSpace(rateProviderConfig.BaseCurrency))
                throw new Exception("ApiConfiguration:BaseCurrency is not set in appsettings.json");

            return rateProviderConfig;
        }
    }
}
