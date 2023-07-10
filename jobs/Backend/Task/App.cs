using ExchangeRateUpdater.Models.Behavior;
using ExchangeRateUpdater.Models.Types;
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
        new Currency(new Code("USD")),
        new Currency(new Code("EUR")),
        new Currency(new Code("CZK")),
        new Currency(new Code("JPY")),
        new Currency(new Code("KES")),
        new Currency(new Code("RUB")),
        new Currency(new Code("THB")),
        new Currency(new Code("TRY")),
        new Currency(new Code("XYZ"))
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
                Console.WriteLine(rate.ToStringFormat());
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

