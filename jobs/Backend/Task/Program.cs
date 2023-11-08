using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;

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
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddConsole(options =>
            {
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            }));
            services.AddTransient<CnbExchangeRateProvider>();
            services.AddHttpClient(nameof(CnbExchangeRateProvider));
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();
            services.AddSingleton(config);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            try
            {
                CnbExchangeRateProvider provider = serviceProvider.GetRequiredService<CnbExchangeRateProvider>();
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
