using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Domain.ValueObjects;
using ExchangeRateUpdater.Infrastructure.Services.Interfaces;
using ExchangeRateUpdater.Infrastructure.Services;
using ExchangeRateUpdater.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Domain.Entities;
using LanguageExt;


namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> currencies = new[]
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
                var provider = InitializeExchangeRateProvider();
                var result = provider.GetExchangeRates(ExchangeRateRoutes.Daily, currencies: currencies);

                var (message, rates) = result.Match<(string Message, List<ExchangeRate> Rates)>(
                    x => new($"Successfully retrieved {x.Count()} exchange rates:", x.ToList()),
                    e => new($"Could not retrieve exchange rates: '{e.Message}'.", [])
                );

                Console.WriteLine(message);
                foreach (var rate in rates)
                    Console.WriteLine(rate.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static IExchangeRateProvider InitializeExchangeRateProvider()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var services = new ServiceCollection();
            services.Configure<Settings>(x =>
            {
                x.DefaultCurrency = configuration["DefaultCurrency"];
                x.CnbUrl = configuration["cnbUrl"];
            });
            services.AddSingleton<ExchangeRateCacheManager>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddMemoryCache();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IExchangeRateProvider>();
        }
    }
}
