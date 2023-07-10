using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater;

internal class App
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
    }

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

    internal void Run(string[] args)
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var rates = provider.GetExchangeRates(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            _logger.LogError(ex, "Unhandled exception occured.");
        }

        Console.ReadLine();
    }
}

