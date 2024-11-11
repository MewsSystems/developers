using Cnb.Api.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            try
            {

                var host = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddEnvironmentVariables();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddLogging(log =>
                        {
                            log.AddConsole();
                        });

                        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                        services.AddHttpClient<ICnbApiClientFactory, CnbApiClientFactory>();
                        services.AddTransient<ICnbApiClientFactory, CnbApiClientFactory>();
                        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

                    })
                    .Build();

                IExchangeRateProvider exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();

                var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

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
