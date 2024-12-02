using ExchangeRates.ConsoleTestProgram;
using ExchangeRates.Core.Models.Configuration;
using ExchangeRates.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        /// <summary>
        /// Main console app for testing the implementation of the exchange rate provider.
        /// It uses the exchange rate provider to retrieve exchange rates for a list of currencies and prints them to the console.
        /// </summary>
        public static void Main(string[] args)
        {
            var serviceProvider = SetupDependencies();
            var provider = serviceProvider.GetService<IExchangeRateProvider>();

            try
            {
                var rates = provider.GetExchangeRates(TestData.Currencies);

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

        /// <summary>
        /// Setup dependencies for the console app.
        /// </summary>
        private static ServiceProvider SetupDependencies()
        {
            // Build configuration, incude appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Register services for dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                .AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>()
                .Services
                .Configure<ExchangeRateSettings>(configuration.GetSection("ExchangeRateSettings"))
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}