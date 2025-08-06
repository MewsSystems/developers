using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Domain.Models;

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
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Configure services
            var services = ServiceConfiguration.ConfigureServices(configuration);
            var serviceProvider = services.BuildServiceProvider();

            try
            {
                // Get the service from DI container
                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();
                var rates = await provider.GetExchangeRates(currencies);

                var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
                Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");
                foreach (var rate in exchangeRates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
            finally
            {
                // Dispose the service provider
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            Console.ReadLine();
        }
    }
}
