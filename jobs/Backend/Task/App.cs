using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Factories;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class App
{
    private readonly TextWriter _output;
    private readonly ExchangeRateProviderFactory _factory;
    private readonly ILogger<App> _logger;

    private static readonly IEnumerable<Currency> currencies = new[]
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

    public App(
        ILogger<App> logger,
        TextWriter output,
        ExchangeRateProviderFactory factory)
    {
        _logger = logger;
        _output = output;
        _factory = factory;
    }

    public async Task Run()
    {
        try
        {
            _logger.LogDebug("App executing");
            var provider = _factory.CreateProvider(CountryIsoAlpha3.CZE);
            var rates = await provider.GetExchangeRates(currencies);
            _output.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                _output.WriteLine(rate.ToString());
            }
            _logger.LogDebug("App finalize.");
        }
        catch (Exception e)
        {
            _output.WriteLine($"Could not retrieve exchange rates: '{e.Message}'");
        }
    }
}
