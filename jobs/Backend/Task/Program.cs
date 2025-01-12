using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

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
            })
            .Build();

        var provider = host.Services.GetRequiredService<ExchangeRateProvider>();

        try
        {
            var date = DateTime.Now;
                
            var rates = await provider.GetExchangeRates(Currencies, date);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates for {date.ToShortDateString()}:");

            Console.WriteLine("{0,-20} | {1,-20} | {2,10}", "Source Currency", "Target Currency", "Rate");
            Console.WriteLine(new string('-', 60));
            foreach (var rate in rates)
            {
                Console.WriteLine("{0,-20} | {1,-20} | {2,10:F4}", rate.SourceCurrency, rate.TargetCurrency, rate.Value);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}