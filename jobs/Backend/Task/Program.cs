using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.ExchangeRateProviders;
using ExchangeRateUpdater.RateProviders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private const string RateProvidersConfigurationKey = "RateProviders";

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
                var applicationConfiguration = new ApplicationConfiguration();
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build()
                    .Bind(applicationConfiguration);
                
                if(!applicationConfiguration.RateProviders.Any())
                {
                    throw new Exception($"Missing configuration for '{RateProvidersConfigurationKey}'");
                }               

                var rateProvider = new CzechNationalBankRateProvider(applicationConfiguration.RateProviders);

                var provider = new ExchangeRateProvider(rateProvider);
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
            }

            Console.ReadLine();
        }
    }
}
