using ExchangeRateUpdater.Exchanges;
using ExchangeRateUpdater.Exchanges.Providers;
using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                var builder = new ConfigurationBuilder();

                IConfiguration config;
                try
                {
                    // Set up configs
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true);
                    config = builder.Build();
                }
                catch (Exception ex)
                {
                    throw;
                }

                // Set up logging
                using ILoggerFactory factory = LoggerFactory.Create(builder =>
                {
                    builder.AddConfiguration(config.GetSection("Logging"));
                    builder.AddConsole();
                });

                ILogger logger = factory.CreateLogger("ExchangeRateUpdater");

                // Set up http wrapper
                var httpResilientClient = new HttpResilientClient(logger);

                // Set up services
                var provider = ExchangeFactory.GetExchangeRateProvider(config, httpResilientClient, logger);
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
