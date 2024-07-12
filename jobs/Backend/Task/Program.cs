using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CzechNationalBankApi;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Application;
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

        public async static Task Main(string[] args)
        {
            //Build a default host, letting .net do the heavy lifting by registring a ILoggerFactory for ILogger<T>,
            //appsettings loading and a container etc :-)
            var host = Host.CreateDefaultBuilder();

            //Register services
            host.ConfigureServices((context, services) => {

                services.AddCzechBankApiService(context.Configuration);
                services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

            });

            var builtHost = host.Build();

            using var serviceScope = builtHost.Services.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            var logger = loggerFactory!.CreateLogger("Program");

            var exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

            try
            {
                logger.LogInformation($"Running GetExchangeRates()....");

                var rates = exchangeRateProvider.GetExchangeRates(currencies);

                logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates:");

                foreach (var rate in rates)
                {
                    logger.LogInformation(rate.ToString());
                }

                logger.LogInformation("Awaiting built host.....");

                //We could omit this for this program, as it doesn't register any HostedServices, but leaving this here :-)
                await builtHost.RunAsync();

                logger.LogInformation("Termination triggered");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Could not retrieve exchange rates: '{ex.Message}'.");

                Environment.Exit(-1);
            }

            //0 for success, anything else means error!
            //This means if we are running this in an orchestrator container (like Azure Kubernetes) it can recover from a bad run because it knows!!
            Environment.Exit(0);
        }
    }
}
