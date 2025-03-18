using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var services = new ServiceCollection();

            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddScoped<IExchangeRateProvider>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                TimeSpan cacheDuration = TimeSpan.FromHours(24);
                return new CnbExchangeRateProvider(httpClient, memoryCache, cacheDuration);
            });
            services.AddScoped<IExchangeRateService, ExchangeRateService>();

            var serviceProvider = services.BuildServiceProvider();

            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

            try
            {
                var rates = await exchangeRateService.GetExchangeRatesAsync(currencies);

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
