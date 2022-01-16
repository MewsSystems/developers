using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public static async Task Main(string[] args)
        {
            try
            {
                GetServiceProvider(args, out IHost host, out IServiceScope serviceScope, out IServiceProvider serviceProvider);

                var exchangeRateUpdaterService = serviceProvider.GetRequiredService<IExchangeRateUpdaterService>();

                var cnbRates = await exchangeRateUpdaterService.GetExchangeRates(currencies.Select(c => c.Code), DateTime.Now);

                Console.WriteLine($"Successfully retrieved {cnbRates.Count()} exchange rates:");
                foreach (var rate in cnbRates)
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

        private static void GetServiceProvider(string[] args, out IHost host, out IServiceScope serviceScope, out IServiceProvider serviceProvider)
        {
            host = HostBuilder.CreateHostBuilder(args).Build();
            serviceScope = host.Services.CreateScope();
            serviceProvider = serviceScope.ServiceProvider;
        }
    }
}