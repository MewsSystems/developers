using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Services;
using ExchangeRateUpdater.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Console
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> _currencies = new[]
        {
            new Currency(CurrencyCode.USD),
            new Currency(CurrencyCode.EUR),
            new Currency(CurrencyCode.CZK),
            new Currency(CurrencyCode.JPY),
            new Currency(CurrencyCode.KES),
            new Currency(CurrencyCode.RUB),
            new Currency(CurrencyCode.THB),
            new Currency(CurrencyCode.TRY)
        };

        public static async Task Main(string[] args)
        {
            var configuration = BuildConfiguration();

            // Add services to the container.
            var serviceProvider = BuildServiceProvider(configuration);

            try
            {
                var service = serviceProvider.GetRequiredService<IExchangeRateService>();

                var responseDto = await service.GetExchangeRatesAsync(_currencies, CurrencyCode.CZK);

                System.Console.WriteLine($"Successfully retrieved {responseDto.ExchangeRates.Count()} exchange rates:");
                foreach (var rate in responseDto.ExchangeRates)
                {
                    System.Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static IServiceProvider BuildServiceProvider(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddCache()
                .ConfigureApplicationSettings(configuration)
                .AddClients()
                .AddServices()
                .AddLogging(b => b.AddConsole())
                .AddMonitoring()
                .BuildServiceProvider();
        }
    }
}
