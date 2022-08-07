using System;
using System.Collections.Generic;
using Domain.Entities;
using ExchangeRateUpdater.Host.Console.Configuration;
using ExchangeRateUpdater.Host.Console.Configuration.Logging;

namespace ExchangeRateUpdater.Host.Console
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
            const string ApplicationName = "ExchangeRateUpdater";
      
            var settingsConfiguration = Settings.GetSettingsConfiguration();
            var settings = Settings.From(settingsConfiguration, ApplicationName);
            
            var logger = SerilogConfiguration.Create(ApplicationName, settings);
            logger.Information("Logger created.");

            var servicesProviderConfiguration = new ServicesProviderConfiguration();
            servicesProviderConfiguration.SetupServices(settings, logger);

            var exchangeRatesSearcherService = servicesProviderConfiguration.GetExchangeRatesSearcherService();
            exchangeRatesSearcherService.GetExchangeRates(currencies);
            
            try
            {
                // TODO [07/08/2022] AR - get the exchange rates given the currencies
                // loop over the exchange rates and write them in the console
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            }

            System.Console.ReadLine();
        }
    }
}
