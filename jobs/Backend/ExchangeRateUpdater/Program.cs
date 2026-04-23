using ExchangeRateUpdater.Application.Banks;
using ExchangeRateUpdater.Application.ExchangeProvider;
using ExchangeRateUpdater.Domain.Constants;
using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Settings;
using ExchangeRateUpdater.Infrastructure.Connectors;
using ExchangeRateUpdater.Infrastructure.Factories;
using ExchangeRateUpdater.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Uncomment following line to override the active bank for this run
            // args = new string[] { "ExchangeRateSettings:ActiveBank=2" };

            var host = InitializeAndBuildHost(args);

            try
            {
                using var scope = host.Services.CreateScope();

                var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                var currencies = appSettings.Currencies.Select(currency => new Currency(currency));
                if (currencies is null || !currencies.Any())
                {
                    Console.WriteLine("No currencies to retrieve exchange rates for.");
                    Console.ReadLine();
                    return;
                }

                var exchangeRateProviderService = scope.ServiceProvider.GetRequiredService<IExchangeRateProviderService>();
                var rates = await exchangeRateProviderService.GetExchangeRates(currencies);

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

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static IHost InitializeAndBuildHost(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var settings = context.Configuration.GetSection("ExchangeRateSettings").Get<AppSettings>();
                    services.AddSingleton(settings);

                    services.AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>();
                    services.AddTransient<IBankFactory, BankFactory>();

                    // Add all banks needed
                    services.AddSingleton<IBankConnector, CzechNationalBankConnector>();
                    services.AddSingleton<IBankConnector, DeNederlandscheBankConnector>();

                    // Add more clients here (for each connector)
                    services.AddHttpClient(BankConstants.CzechNationalBank.HttpClientIdentifier)
                        .ConfigureHttpClient((sp, client) =>
                        {
                            var cnbBankSettings = settings.BankConfigurations.Where(bank => bank.Id == BankIdentifier.CzechNationalBank).First();
                            client.BaseAddress = new Uri(cnbBankSettings.ExchangeRateApiUrl);
                        });
                })
                .Build();

            return host;
        }
    }
}
