using ExchangeRateProvider.Contracts;
using ExchangeRateProviderCzechNationalBank;
using ExchangeRateProviderCzechNationalBank.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public async static Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.AddConsole();
            });
            services.RegisterExchangeRateProviderForCzechNationalBank();

            try
            {
                var serviceProvider = services.BuildServiceProvider();
                var provider = serviceProvider.GetService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);

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
