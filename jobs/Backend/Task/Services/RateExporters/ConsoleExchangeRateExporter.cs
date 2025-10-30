using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services.RateExporters;

/// <summary>
///     Exports the rate information to the console
/// </summary>
public class ConsoleExchangeRateExporter : IExchangeRateExporter
{
    private readonly ILogger<ConsoleExchangeRateExporter> _logger;

    public ConsoleExchangeRateExporter(ILogger<ConsoleExchangeRateExporter> logger)
    {
        _logger = logger;
    }

    public async Task ExportExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates)
    {
        _logger.LogInformation($"Successfully retrieved {exchangeRates.Count()} exchange rates:");

        foreach (var rate in exchangeRates) await Console.Out.WriteLineAsync(rate.ToString());
    }
}