using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logger;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public class Program
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

        public static async Task Main()
        {
            ILogger logger = new LoggerService().logger;

            try
            {
                ExchangeRateProvider provider = new ExchangeRateProvider();
                List<ExchangeRate> exchangeRates = await provider.GetExchangeRates(currencies, logger);

                Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");
                foreach (ExchangeRate exchangeRate in exchangeRates)
                {
                    Console.WriteLine(exchangeRate.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Program encountered an unhandled exception: {ex.Message}");
                Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
