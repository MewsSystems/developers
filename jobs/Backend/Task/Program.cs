using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        // I changed it from IEnumerable to HashSet because we do not need lazy iteration
        // and the number of possible currencies is small, so there is no memory issue.
        // Also HashSet is faster for searches
        private static readonly HashSet<Currency> Currencies = new()
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
                await using var serviceProvider = HostHelper.CreateServiceProvider();
                var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(Currencies);

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
