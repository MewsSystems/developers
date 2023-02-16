using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRateUpdater;

public sealed class ExchangeRatePrinter : IExchangeRatePrinter
{
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ILogger<ExchangeRatePrinter> _logger;

    public ExchangeRatePrinter(IExchangeRateProvider exchangeRateProvider, ILogger<ExchangeRatePrinter> logger)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _logger = logger;
    }

    public async Task PrintRates(IEnumerable<Currency> currencies)
    {
        var rates = _exchangeRateProvider.GetExchangeRatesAsync(currencies);
        var ratesCount = 0;
        var ratesStringBuilder = new StringBuilder();
        await foreach (var rate in rates)
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
