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

                // Don't advertise our stack trace and error to the front end user
                Console.WriteLine($"Failed to retrieve Exchange Rates. Please try again.");
            }

            Console.ReadLine();
        }
    }
}
