using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.ExchangeRateApi;
using Microsoft.Extensions.Configuration;
using NLog;

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
            // Ensure NLog is initialized and test logging
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Application started.");

            try
            {
                var provider = new ExchangeRateProvider(GetRateProviderConfiguration());
                var rates = await provider.GetExchangeRatesAsync<CnbApiResponse>(currencies);

                // Print CNB results as returned
                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates from CNB:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }

                #region Another Source of Exchange Rates
                //var anotherProvider = new ExchangeRateApiProvider(GetExchangeRateApiProviderConfiguration());
                //var anotherRates = await anotherProvider.GetExchangeRatesAsync<ExchangeRateApiResponse>(currencies);

                //// Print ExchangeRate-API results as returned
                //Console.WriteLine($"Successfully retrieved {anotherRates.Count()} exchange rates from Exchange Rates API:");
                //foreach (var rate in anotherRates)
                //{
                //    Console.WriteLine(rate.ToString());
                //}
                #endregion

            }
            catch (Exception e)
            {
                logger.Error($"Could not retrieve exchange rates: '{e.Message}'.");

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

        private static IExchangeRateProviderConfiguration GetExchangeRateApiProviderConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var rateProviderConfig = new ExchangeRateProviderConfiguration
            {
                Url = configuration["ApiConfiguration:AnotherUrl"] + configuration["ApiConfiguration:AnotherBaseCurrency"],
                BaseCurrency = configuration["ApiConfiguration:AnotherBaseCurrency"]
            };

            if (string.IsNullOrWhiteSpace(rateProviderConfig.Url))
                throw new Exception("ApiConfiguration:AnotherUrl is not set in appsettings.json");

            if (string.IsNullOrWhiteSpace(rateProviderConfig.BaseCurrency))
                throw new Exception("ApiConfiguration:AnotherBaseCurrency is not set in appsettings.json");

            return rateProviderConfig;
        }
    }
}
