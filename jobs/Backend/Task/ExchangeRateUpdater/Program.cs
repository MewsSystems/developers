using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeRateProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

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

    private static IHost BuildHost()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");

        builder.ConfigureServices();
        builder.AddCacheAndRetryPolicy();
        builder.Services.AddServices();

        return builder.Build();
    }

    private static async Task GetRatesAsync(IExchangeRateProvider provider)
    {
        var rates = (await provider.GetExchangeRatesAsync(Currencies)).ToArray();

        Console.WriteLine($"Successfully retrieved {rates.Length} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }
    }

    public static async Task Main(string[] args)
    {
        using var host = BuildHost();
        var provider = host.Services.GetRequiredService<IExchangeRateProvider>();

        try
        {
            Console.WriteLine();
            await GetRatesAsync(provider);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
        
        Console.ReadLine();
    }
}