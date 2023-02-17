using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using System;
using System.Collections.Generic;
using System.Text;
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
        var ratesCount = 0;
        var ratesStringBuilder = new StringBuilder();
        foreach (var rate in rates)
        {
            ratesStringBuilder.Append(rate);
            ratesStringBuilder.AppendLine();
            ratesCount++;
        }
        Console.WriteLine($"Successfully retrieved {ratesCount} exchange rates:");
        Console.WriteLine(ratesStringBuilder.ToString());

        Console.ReadKey();
    }
}
