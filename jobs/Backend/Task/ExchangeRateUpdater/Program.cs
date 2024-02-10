using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
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
        private static readonly IEnumerable<ICurrency> currencies = new[]
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
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((configuration) =>
                {
                    configuration.AddJsonFile($"appsettings.json", false, true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    ExchangeRateConfiguration config = hostContext.Configuration.Get<ExchangeRateConfiguration>();
                    services.AddHttpClient<ICzechNationalBankClient, CzechNationalBankClient>(a => a.BaseAddress = config.BaseUrl);
                    services.AddScoped<IExchangeRateService, ExchangeRateService>();
                })
                .UseConsoleLifetime();

            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    IExchangeRateService service = services.GetRequiredService<IExchangeRateService>();
                    IEnumerable<IExchangeRate> rates = await service.GetExchangeRatesAsync("CZK", currencies);

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
}
