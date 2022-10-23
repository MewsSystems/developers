using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            try
            {
                var container = SetupContainer();
                var provider = container.GetRequiredService<IExchangeRateProvider>();

                var rates = await provider.GetExchangeRates(currencies);

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

        private static IServiceProvider SetupContainer()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            var config = builder.AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();

            var cnbClientOptions = config.GetSection("CnbClient").Get<CnbClient.Options>();
            services.AddSingleton(cnbClientOptions);

            var exchangeRateCacheOptions = config.GetSection("ExchangeRateCache").Get<ExchangeRateCache.Options>();
            services.AddSingleton(exchangeRateCacheOptions);

            services.AddSingleton<ICnbClient, CnbClient>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();

            services.AddMemoryCache();

            return services.BuildServiceProvider();
        }
    }
}
