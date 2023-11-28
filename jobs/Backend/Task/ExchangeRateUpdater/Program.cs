using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.DependencyInjection;

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

    public static async Task Main(string[] args)
    {
        try
        {
            var provider = BuildExchangeRateProvider();
            var rates = await provider.GetExchangeRates(Currencies);

            var exchangeRateList = rates.ToList();
            Console.WriteLine($"Successfully retrieved {exchangeRateList.Count} exchange rates:");
            foreach (var rate in exchangeRateList)
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

    private static IExchangeRateProvider BuildExchangeRateProvider()
    {
        var services = new ServiceCollection();
        services.AddConfiguration();
        services.AddExchangeRateUpdaterServices();
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IExchangeRateProvider>();
    }
}