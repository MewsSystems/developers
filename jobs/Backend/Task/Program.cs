using ExchangeRateProvider.Models;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.DependencyInjection;
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
                var serviceCollection = new ServiceCollection();

                serviceCollection.AddTransient<IExchangeDataSourceFactory, ExchangeDataSourceFactory>();
                serviceCollection.AddTransient<IExchangeRateProvider>(provider =>
                {
                    var factory = provider.GetRequiredService<IExchangeDataSourceFactory>();
                    return new ExchangeRateProvider(factory.CreateDataSource(ExchangeRateDataSourceType.Cnb));

                });

                var serviceProvider = serviceCollection.BuildServiceProvider();

                var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine($"Could not create data source: '{e.Message}'.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
