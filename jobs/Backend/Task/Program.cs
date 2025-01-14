using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

[ExcludeFromCodeCoverage]
public static class Program
{
    private static readonly IEnumerable<Currency> Currencies =
    [
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    ];

    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
                services.AddSingleton<ExchangeRateProvider>();
                services.AddSingleton(TimeProvider.System);
            })
            .Build();

        var provider = host.Services.GetRequiredService<ExchangeRateProvider>();

        try
        {
            var date = DateTime.UtcNow;
            var targetCurrency = new Currency("CZK", "koruna");

            var rates = await provider.GetExchangeRates(date, targetCurrency, Currencies);

            if (rates.Any())
            {
                Console.WriteLine(
                    $"Successfully retrieved {rates.Count} exchange rates for {date.ToShortDateString()}:");

                Console.WriteLine("{0,-20} | {1,-20} | {2,10} | {3,10}", 
                    "Source Currency", "Target Currency", "Rate", "Valid For");
                
                Console.WriteLine(new string('-', 70));
                foreach (var rate in rates)
                {
                    Console.WriteLine("{0,-20} | {1,-20} | {2,10:F4} | {3,10}", 
                        rate.SourceCurrency, 
                        rate.TargetCurrency,
                        rate.Value.ToString("F2"),
                        rate.ValidFor.ToShortDateString());
                }
                Console.WriteLine(new string('-', 70));

                Console.WriteLine("*Exchange rates are updated each business day at 2:30 PM Central European Time.");
                Console.WriteLine(ExchangeRateHelper.GetTimeUntilNextExchangeRateData());
            }
            else
            {
                Console.WriteLine(
                    $"No exchange rates for were found for {date.ToShortDateString()}.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates due to an exception: '{e.Message}'.");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadLine();
    }
}