using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];

        public static async Task Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var serviceProvider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddScoped<IExchangeRateProvider, ExchangeRateProvider>()
                .BuildServiceProvider();

            IExchangeRateProvider exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            try
            {
                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

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
