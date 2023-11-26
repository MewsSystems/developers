using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Services;
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

    public static void Main(string[] args)
    {
        try
        {
            var serviceProvider = DependencyResolver.Initialize();

            var provider = serviceProvider.GetService<IExchangeRateProvider>();
            var rates = provider.GetExchangeRates(Currencies);

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