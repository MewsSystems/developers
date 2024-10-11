using System;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
            .AddSingleton<IExchangeRateClient, ExchangeRateClient>()
            .BuildServiceProvider();

			      try
            {
                var rateProvider = serviceProvider.GetService<IExchangeRateProvider>();
                var rates = await rateProvider.GetExchangeRatesAsync(ExchangeRateSettings.Currencies);

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
