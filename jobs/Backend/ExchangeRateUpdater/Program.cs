using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Settings;

namespace ExchangeRateUpdater;

public class Program
{
    private const string TargetCurrencyKey = "TargetCurrencyCode";
    private static readonly IReadOnlyCollection<Currency> Currencies = new[]
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
    
    public static async Task Main()
    {
        try
        {
            var host = CreateHostBuilder().Build();
            var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
            var rates = await provider.GetExchangeRatesAsync(Currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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

    #region private methods

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddOptions<AppSettings>().Bind(hostContext.Configuration.GetSection(nameof(AppSettings)));
                var targetCurrency = hostContext.Configuration.GetValue<string>(TargetCurrencyKey);

                switch (targetCurrency)
                {
                    case "CZK":
                        services.AddScoped<IExchangeRateProvider, CzechBankExchangeRateProvider>();
                        services.AddScoped<IExchangeRateService, CzechBankExchangeRateService>();
                        services.AddScoped<ICurrencyParser, CzechBankCurrencyParser>();
                        break;
                    case "PLN":
                        // Inject PLN services
                        break;
                    default:
                        break;
                }
            });

    #endregion
}