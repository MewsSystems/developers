using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRate.Core;
using ExchangeRate.Core.Extentions;
using ExchangeRate.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            try
            {
                var hostBuilder = CreateHostBuilder(args);

                var providerFactory = hostBuilder
                    .Build()
                    .Services
                    .GetRequiredService<ExchangeRateProvidersFactory>();

                var rates = await providerFactory.GetExchangeRatesAsync(currencies, "CNB");

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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    services.AddExchangeRateProvider(settings => configuration.GetSection("CNBSettings").Bind(settings));
                })
                .UseConsoleLifetime();
        }
    }
}
