using ExchangeRateUpdater.ExchangeRateStrategies.Cnb;
using ExchangeRateUpdater.ExchangeRateStrategies.CurrencyLayer;
using ExchangeRateUpdater.ExchangeRateStrategies.Fixer;
using ExchangeRateUpdater.Factory;
using System;
using System.Collections.Generic;
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
            // create sample configuration with some implementaions of providers
            var config = new DefaultConfig()
                .AddTargetCurrencyProvider(new Currency("CZK"), () => new CnbExchangeRateProviderStrategy(
                    new CnbRatesHttpClientFetcher(),
                    new CnbRatesTxtParser()))
                .AddSourceCurrencyProvider(new Currency("EUR"), () => new FixerExchangeRateProviderStrategy(
                    new FixerHttpClientRatesFetcher()))
                .AddSourceCurrencyProvider(new Currency("USD"), () => new CurrencyLayerExchangeRateProviderStrategy(
                    new CurrencyLayerHttpClientRatesFetcher()));
            var factory = new ConfigurableProviderFactory(config);
            
            try
            {
                var provider = new ExchangeRateProvider(factory);
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine("Successfully retrieved " + rates.Count() + " exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while retrieving exchange rates: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
