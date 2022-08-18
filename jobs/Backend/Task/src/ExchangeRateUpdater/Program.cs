using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Providers;
using ExchangeRateUpdater.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            Currency.From("USD"),
            Currency.From("EUR"),
            Currency.From("CZK"),
            Currency.From("JPY"),
            Currency.From("KES"),
            Currency.From("RUB"),
            Currency.From("THB"),
            Currency.From("TRY"),
            Currency.From("XYZ")
        };

        public static async Task Main(string[] args)
        {
            try
            {
                var provider = ServicesConfigurationRoot.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
                var rates = (await provider.GetExchangeRates(currencies)).ToList();

                Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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