﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            Console.ReadLine();
        }

        /// <summary>
        /// Helper method to setup DI
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureServiceProvider()
        {
            return new ServiceCollection()
                    .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                    .AddLogging((loggingBuilder) => loggingBuilder
                        .SetMinimumLevel(LogLevel.Trace)//.SetMinimumLevel(LogLevel.Information)
                        .AddConsole()
                    )
                    .BuildServiceProvider();
        }
    }
}
