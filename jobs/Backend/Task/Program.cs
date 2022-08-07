namespace ExchangeRateUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Caching;
    using CurrencyExchangeService.Interfaces;
    using CurrencyExchangeService.Models;
    using CurrencyExchangeService.Utilities;
    using ExchangeRateService;
    using ExchangeRateService.Models;
    using Logger;

    public static class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<ILogger, FileLogger>()
                    .AddSingleton<IExchangeRateService<ExchangeRate, Currency>, CNBExchangeRateService>()
                    .AddSingleton<ISerializationHelper<CurrencyRateXmlResponse>, XmlSerializationHelper>()
                    .AddSingleton<ICacheService<string, string>, InMemCacheService>()
                    .AddSingleton<IExchangeRateProvider<string>, CNBExchangeRateProvider>());
        }

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
            new Currency("XYZ"),
            new Currency("LOL"),
            new Currency("GBP"),
            new Currency("SEK"),
            new Currency("SGD"),
            new Currency("RON"),
            new Currency("PLN"),
            new Currency("NZD"),
            new Currency("NOK"),
            new Currency("XDR"),
            new Currency("XDR"),
        };

        public static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            Execute(host.Services);

            return host.RunAsync();
        }

        public static async void Execute(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var exchangeRateService = provider.GetRequiredService<IExchangeRateService<ExchangeRate, Currency>>();
            var logger = provider.GetRequiredService<ILogger>();

            try
            {
                var rates = await exchangeRateService.GetExchangeRatesAsync(currencies);
                if (rates != null && rates.Any())
                {
                    logger.Info($"Successfully retrieved {rates.Count()} exchange rates:");
                    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

                    foreach (var rate in rates)
                    {
                        logger.Info(rate.ToString());
                        Console.WriteLine(rate.ToString());
                    }
                }
                else
                {
                    logger.Info("No currencies were retrieved");
                    Console.WriteLine("No currencies were retrieved");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Program:Execute: Could not retrieve exchange rates: '{e.Message}'.");
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }
    }
}
