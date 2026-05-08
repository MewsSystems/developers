using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency(IsoCurrencyCode.USD),
            new Currency(IsoCurrencyCode.EUR),
            new Currency(IsoCurrencyCode.CZK),
            new Currency(IsoCurrencyCode.JPY),
            new Currency(IsoCurrencyCode.KES),
            new Currency(IsoCurrencyCode.RUB),
            new Currency(IsoCurrencyCode.THB),
            new Currency(IsoCurrencyCode.TRY)
        };

        public static async Task Main(string[] args)
        {
            IServiceProvider serviceProvider = GetConfiguredServiceProvider();

            try
            {
                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);
                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e}'.");
            }
        }

        private static IServiceProvider GetConfiguredServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddExchangeRatesProvider();
            return services.BuildServiceProvider();
        }
    }
}
