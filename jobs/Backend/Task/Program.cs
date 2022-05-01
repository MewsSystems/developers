using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddCommandLine(args)
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("HttpClientCzechNationalBank", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri(configuration.GetValue<string>("apiExchangeRatesCNBUrl"));
                    })
                    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(3, retryNumber => TimeSpan.FromMilliseconds(600)));
                    services.AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<ExchangeRateProvider>>());
                    services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var exchangeRateProvider = services.GetRequiredService<IExchangeRateProvider>();
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
            }

            Console.ReadLine();
        }
    }
}