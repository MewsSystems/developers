using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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
            var logger = ConsoleLogger.Instance;

            if (!Uri.TryCreate(ConfigurationManager.AppSettings["ExchangeRateEndpointXml"], UriKind.Absolute, out var exchangeRateUri))
            {
                logger.Log("Invalid exchange rate URI. Correct the value in App.config file.");
            }

            IExchangeRateLoader loader = new XmlExchangeRateLoader(
                exchangeRateUri,
                ConsoleLogger.Instance);

            try
            {
                var provider = new ExchangeRateProvider(loader);
                var rates = provider.GetExchangeRates(currencies);

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
