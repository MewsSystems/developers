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

        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                var providerFactory = new ExchangeRateProviderFactory();
                var provider = providerFactory.GetExchangeRateProvider(ExchangeRateProviderType.Cnb);
                var rates = provider.GetExchangeRates(currencies).GetAwaiter().GetResult();

                logger.Info($"Successfully retrieved {rates.Count()} exchange rates:");
                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    logger.Info(rate.ToString());
                    Console.WriteLine(rate.ToString());
                }
                
            }
            catch (Exception e)
            {
                logger.Error(e, $"Could not retrieve exchange rates: '{e.Message}'.");
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
