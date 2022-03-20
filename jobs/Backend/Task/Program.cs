using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Fetch;
using ExchangeRateUpdater.Parse;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            try
            {
                var services = ConfigureServices();
                var provider = services.GetRequiredService<ExchangeRateProvider>();
                
                var rates = provider.GetExchangeRates(currencies);

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

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Error);
                })
                .AddSingleton<IExchangeRatesParser, ExchangeRatesCnbCzParser>()
                .AddSingleton<IExchangeRatesTxtFetcher, ExchangeRatesTxtFetcher>()
                .AddHttpClient()
                .AddSingleton<IConfiguration>(_ =>
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", optional: false);
                    return builder.Build();
                })
                .AddSingleton<Config>(services =>
                {
                    var config = services.GetRequiredService<IConfiguration>();
                    return config.GetSection("Config").Get<Config>();
                })
                .AddSingleton<ExchangeRateProvider>()
                .BuildServiceProvider();
        }
    }
}
