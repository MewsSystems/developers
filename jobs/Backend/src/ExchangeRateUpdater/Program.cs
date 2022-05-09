using Core.Application;
using Core.Application.Interfaces;
using Core.Domain.Models;
using Core.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
              .Build();

            using IHost host = CreateHostBuilder(args, configuration).Build();

            try
            {
                using IServiceScope serviceScope = host.Services.CreateScope();
                IServiceProvider provider = serviceScope.ServiceProvider;

                IExchangeRateProvider exchangeServiceProvider = provider.GetRequiredService<IExchangeRateProvider>();

                var ratesResult = await exchangeServiceProvider.GetExchangeRates(currencies);

                if (ratesResult.IsSuccess)
                {
                    var rates = ratesResult.Value;

                    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"Could not retrieve exchange rates: '{ratesResult.Error}'.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddApplicationServices();
                    services.AddExchangeRateClient(configuration["ExchangeRateClient:BaseAddress"]);
                });
    }
}
