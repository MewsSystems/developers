using ExchangeRatesExporting;
using ExchangeRatesFetching;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRatesUpdater;

internal interface IExchangeRatesCoordinator
{
    Task GetAndExportExchangeRates(IDictionary<string, IEnumerable<string>> banksCurrenciesDictionary);
}

internal class ExchangeRatesCoordinator : IExchangeRatesCoordinator
{
    private readonly IExchangeRatesAggregator exchangeRatesAggregator;
    private readonly IExporter exchangeRatesExporter;
    private readonly ILogger<ExchangeRatesCoordinator> logger;

    public ExchangeRatesCoordinator(IExchangeRatesAggregator exchangeRatesAggregator, IConsoleExporter consoleExporter, ILogger<ExchangeRatesCoordinator> logger)
    {
        this.exchangeRatesAggregator = exchangeRatesAggregator;
        exchangeRatesExporter = consoleExporter;
        this.logger = logger;
    }

    public async Task GetAndExportExchangeRates(IDictionary<string, IEnumerable<string>> banksCurrenciesDictionary)
    {
        try {
            IEnumerable<Common.ExchangeRate> rates = await exchangeRatesAggregator.GetExchangeRates(banksCurrenciesDictionary);
            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            exchangeRatesExporter.Export(rates);
        } catch (Exception e) {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            logger.LogError(e, "Could not retrieve exchange rates.");
        }
    }
}
