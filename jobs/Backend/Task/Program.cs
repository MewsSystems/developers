using Microsoft.Extensions.Hosting;
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

        public async static Task Main(string[] args)
        {
            try
            {
                //Build a default host, letting .net do the heavy lifting by registring a ILoggerFactory for ILogger<T>,
                //appsettings loading and a container etc :-)
                var host = Host.CreateDefaultBuilder();

                //Register services

                //Retrieve from container
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");

                Environment.Exit(-1);
            }

            Console.ReadLine();

            Environment.Exit(0);
        }
    }
}
