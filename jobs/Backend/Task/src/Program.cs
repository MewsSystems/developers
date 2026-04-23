using ExchangeRateUpdater.Core;
using ExchangeRateUpdater.Core.Http;
using ExchangeRateUpdater.Infrastructure.Http;
using ExchangeRateUpdater.Infrastructure.Providers;
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
            var services = new ServiceCollection();

            services.AddHttpClient<IHttpClient, DefaultHttpClient>();
            services.AddTransient<IExchangeRateProvider, CnbExchangeRateProvider>();

            var serviceProvider = services.BuildServiceProvider();

            var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            try
            {
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
