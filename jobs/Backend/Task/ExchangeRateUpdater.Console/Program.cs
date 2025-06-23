using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> Currencies = new[]
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
        var provider = BuildServiceProvider();
        
        var exchangeRateProvider = provider.GetRequiredService<IExchangeRateProvider>();
        await RunAsync(exchangeRateProvider);
    }

    public static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        var config = builder.Build();
        services.AddCoreServices(config);
        return services.BuildServiceProvider();
    }
    
    private static async Task RunAsync(IExchangeRateProvider exchangeRateProvider)
    {
        try
        {
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(Currencies);

            System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
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
}