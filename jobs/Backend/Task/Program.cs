using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.DataSources;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddScoped<IExchangeRateProvider, ExchangeRateProvider>()
                .AddScoped<IExchangeRateDataSource, ExchangeRateDataSource>()
                .AddSingleton<IExchangeRateDataSourceOptions>(new ExchangeRateDataSourceOptionsBuilder().Build())
                .AddHttpClient<IExchangeRateDataSource, ExchangeRateDataSource>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
                .Services
                .AddMemoryCache()
                .BuildServiceProvider();


            try
            {
                var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
                var currencies = GetCurrencies();

                var rates = provider.GetExchangeRates(currencies);

                var count = (await rates).Count();

                Console.WriteLine($"Successfully retrieved {count} exchange rates:");
                foreach (var rate in await rates)
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

        private static Currency[] GetCurrencies()
        {
            return new[]
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
        }
    }
}
