using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure the host builder
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<SettingOptions>(hostContext.Configuration.GetSection("Settings"));
                    // Register services
                    services.AddHttpClient<IHttpWebClient, HttpWebClient>();

                    services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                });


            // Build the host
            using var host = builder.Build();

            try
            {
                var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var settingOptions = host.Services.GetRequiredService<IOptions<SettingOptions>>().Value;
                var allowedCurrenciess = settingOptions.AllowedCurrencies.Select(x => new Currency(x));
                var rates = await provider.GetExchangeRatesAsync(allowedCurrenciess);

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
