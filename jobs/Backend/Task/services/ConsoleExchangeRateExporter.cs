using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.model;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.services;

public class ConsoleExchangeRateExporter : IExchangeRateExporter
{
    private readonly ILogger<ConsoleExchangeRateExporter> _logger;
    
    public ConsoleExchangeRateExporter(ILogger<ConsoleExchangeRateExporter> logger)
    {
        _logger = logger;
    }
    
    public void ExportExchangeRates(IEnumerable<ExchangeRate> exchangeRates)
    {
        _logger.LogInformation($"Successfully retrieved {exchangeRates.Count()} exchange rates:");
        foreach (var rate in exchangeRates)
        {
            Console.WriteLine(rate.ToString());
        }
    }
}