using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            var startup = new Startup();

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(startup.ConfigureServices)
                .Build();

            // Get services
            var logger = host.Services.GetRequiredService<ILoggerFactory>()
                             .CreateLogger("Main");
            var providerservice = host.Services.GetRequiredService<IExchangeRateProviderService>();

            try
            {
                var provider = new ExchangeRateProvider(providerservice);
                var rates = provider.GetExchangeRatesAsync(currencies).Result;

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
