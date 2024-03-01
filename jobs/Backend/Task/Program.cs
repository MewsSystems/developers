using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Helpers;
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

        public static void Main(string[] args)
        {
            var logger = new Logger().logger;

            try
            {
                var provider = new ExchangeRateProvider(logger);
                var rates = provider.GetExchangeRates(currencies);

                if (rates == null)
                {
                    logger.LogInformation("No exchange rates were retrieved.");
                    return;
                }

                logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    logger.LogInformation(rate.ToString());
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
