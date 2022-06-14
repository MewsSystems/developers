using System;
using System.Collections.Generic;
using System.Linq;
using models.ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public static class Program
    {


        public static void Main(string[] args)
        {


            using IHost host = CreateHostBuilder(args).Build();
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();
            var exchangeRateValidationsHelper = host.Services.GetService<IExchangeRateValidationsHelper>();

            var keepRunning = true;
            var currencies = new List<CurrencyModel>();
            int maxInputCount = 0;
            while (keepRunning)
            {
                Console.Write("Please enter a currency of interest or type STOP to finish: ");
                var userInput = Console.ReadLine().ToUpper();

                if (userInput == "STOP" || maxInputCount == 9)
                {
                    keepRunning = false;
                }
                else
                {
                    exchangeRateValidationsHelper.ValidateCurrency(userInput, settings);
                    currencies.Add(new CurrencyModel(userInput));
                    maxInputCount++;
                }

            }
            var rates = host.Services.GetService<IExchangeServiceProviderService>().GetExchangeRates(currencies, settings);

            if (rates.Count() > 0)
            {
                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            else{
                Console.WriteLine($"Not valid currencies were input");

            }

        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IExchangeServiceProviderService, ExchangeRateProviderService>();
                    services.AddScoped<IExchangeRateValidationsHelper, ExchangeRateValidationsHelper>();

                });

            return hostBuilder;
        }

    }
}
