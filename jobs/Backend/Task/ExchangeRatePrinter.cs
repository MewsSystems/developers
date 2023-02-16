using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        await foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }

        Console.ReadKey();
    }
}
