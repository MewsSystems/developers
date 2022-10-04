using Microsoft.Extensions.DependencyInjection;
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

        public static async Task Main(string[] args)
        {
            try
            {
                // configure service provider
                var serviceProvider = ConfigureServiceProvider();

                // obtain instance of ExchangeRateProvider
                var exchangeRateProvider = serviceProvider.GetService<IExchangeRateProvider>();

                // get the exchange rates for the given currencies
                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

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

        /// <summary>
        /// Helper method to setup DI
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureServiceProvider()
        {
            return new ServiceCollection()
                    .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                    .BuildServiceProvider();
        }
    }
}
