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
            var serviceProvider = new ServiceCollection()
              .AddSingleton<IExchangeRatesListingParser, ExchangeRatesListingParser>()
              .AddSingleton<ICnbApiClient, CnbApiClient>()
              .AddSingleton<IExchangeRatesListingsCache, ExchangeRatesListingsCache>()
              .AddSingleton<IBankDateProvider, BankDateProvider>()
              .AddSingleton<IDateTimeProvider, DateTimeProvider>()
              .AddSingleton<ExchangeRateProvider>()
              .BuildServiceProvider();

            try {
                var provider = serviceProvider.GetService<ExchangeRateProvider>();
                var rates = provider.GetExchangeRates(currencies).Result;

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
