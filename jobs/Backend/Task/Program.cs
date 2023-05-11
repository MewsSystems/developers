using ExchangeRateUpdater.BL.Implementations;
using ExchangeRateUpdater.BL.Interfaces;
using ExchangeRateUpdater.BL.Models;
using ExchangeRateUpdater.DAL.Implementations;
using ExchangeRateUpdater.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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
                //Create a service collection to inject Logging intor the Class Libraries
                var services = new ServiceCollection();
                services.AddLogging(configure => configure.AddConsole());
                services.AddTransient<IDataScrapper, DataScrapper>();
                services.AddTransient<ExchangeRateUpdaterService>();
                ServiceProvider serviceProvider = services.BuildServiceProvider();

                IExchangeRateUpdaterService _exchangeRateUpdaterService = serviceProvider.GetRequiredService<ExchangeRateUpdaterService>();
                var provider = new ExchangeRateProvider(_exchangeRateUpdaterService);
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
    }
}
