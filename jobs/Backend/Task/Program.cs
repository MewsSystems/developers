using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var serviceProvider = Configure();

                var exchangeProvider = serviceProvider.GetService<IExchangeRateProvider>();
                var currencyProvider = serviceProvider.GetService<ICurrencyProvider>();

                var currencies = currencyProvider.Get();

                var rates = (await exchangeProvider.GetExchangeRates(currencies, DateTime.Now, CancellationToken.None)).ToList();

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

        private static ServiceProvider Configure()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                        .AddConsole();
                })
                .AddExchangeProviders(configuration)
                .BuildServiceProvider();
        }
    }
}
