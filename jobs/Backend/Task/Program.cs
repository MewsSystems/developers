using BenchmarkDotNet.Running;
using ExchangeRateUpdater.Benchmark;
using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

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
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var provider = services.GetRequiredService<IExchangeRateService>();

                bool exit = false;
                do
                {
                    Console.WriteLine("1. Get Exchange Rates");
                    Console.WriteLine("2. Exit");
                    Console.Write("Choose an option: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            await DisplayExchangeRates(provider);
                            break;
                        case "2":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }

                    Console.WriteLine();

                } while (!exit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            }
        }

    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args);

        host.ConfigureServices(x =>
        {
            x.AddLogging(builder => builder.AddConsole());
            x.AddMemoryCache();
            x.AddHttpClient<IExchangeRateRetriever, ExchangeRateRetriever>();
            x.AddScoped<IExchangeRateService, ExchangeRateService>();
            x.AddScoped<IExchangeRateMapper, ExchangeRateMapper>();
            x.AddScoped<IExchangeRateCache, ExchangeRateCache>();
            x.AddScoped<IConfiguration>(x => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
        });

        return host;
    }

    private static async Task DisplayExchangeRates(IExchangeRateService service)
    {
        try
        {
            var rates = await service.GetExchangeRatesAsync(currencies);

            if (rates.IsSuccess)
            {
                Console.WriteLine($"Successfully retrieved {rates.Value.Count()} exchange rates:");
            
                foreach (var rate in rates.Value)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            else
            {
                Console.WriteLine($"Error: {rates.Error}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }
}
