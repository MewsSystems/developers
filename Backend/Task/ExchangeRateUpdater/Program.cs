using Microsoft.Extensions.DependencyInjection;
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
            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddHttpClient().AddTransient(a => currencies)
                    .AddTransient<CNBExchangeRateProvider>()
                    .BuildServiceProvider();
                var service = serviceProvider.GetService<CNBExchangeRateProvider>();

                var rates = service.GetExchangeRates();

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'- '{e.InnerException.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
