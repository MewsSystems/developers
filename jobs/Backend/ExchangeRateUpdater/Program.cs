using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Program
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
                // configure service provider
                var serviceProvider = ConfigureServiceProvider();
                
                // setup Logger instance
                var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

                try
                {
                    // obtain instance of ExchangeRateProvider
                    var exchangeRateProvider = serviceProvider.GetService<IExchangeRateProvider>();

                    // get the exchange rates for the given currencies
                    var rates = await exchangeRateProvider.GetExchangeRates(currencies);

                    if (rates.Any())
                    {
                        logger.LogInformation($"Retrieved {rates.Count()} exchange rates:");
                        foreach (var rate in rates)
                        {
                            logger.LogInformation(rate.ToString());
                        }
                    } 
                    else
                    {
                        logger.LogInformation($"No rates retrieved for the given currencies.");
                    }
                } 
                catch (Exception ex)
                {
                    logger.LogError($"Failed to retrieve exchange rates: {Environment.NewLine}");
                    logger.LogError(ex, ex.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception: '{e.Message}'.");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
                Console.WriteLine("Goodbye.");
            }
        }

        /// <summary>
        /// Helper method to setup DI
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureServiceProvider()
        {
            var configuration = SetupConfiguration();

            return new ServiceCollection()
                    .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                    .AddSingleton<IConfiguration>(configuration)
                    .AddLogging((loggingBuilder) => loggingBuilder
                        .SetMinimumLevel(LogLevel.Trace)//.SetMinimumLevel(LogLevel.Information)
                        .AddConsole()
                    )
                    .BuildServiceProvider();
        }

        /// <summary>
        /// Helper method to setup configuration
        /// </summary>
        /// <returns></returns>
        private static IConfigurationRoot SetupConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile($"appSettings.{environment}.json")
                     .AddEnvironmentVariables()
                     .Build();
        }
    }
}
