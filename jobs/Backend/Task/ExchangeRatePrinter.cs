using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public sealed class ExchangeRatePrinter : IExchangeRatePrinter
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public ExchangeRatePrinter(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task PrintRates(IEnumerable<Currency> currencies)
    {
        var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate);
        }
        Console.ReadKey();
    }
}
