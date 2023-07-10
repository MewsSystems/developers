using ExchangeRateUpdater.Models.Behavior;
using ExchangeRateUpdater.Models.Errors;
using ExchangeRateUpdater.Models.Types;
using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

internal class App
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger,
        IExchangeRateRepository exchangeRateRepository,
        IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _exchangeRateProvider = exchangeRateProvider;
        _logger = logger;
    }

    internal async Task<int> Run(string[] args)
    {
        try
        {
            var sourceCurrencies = _exchangeRateRepository.GetSourceCurrencies();
            var exchangeRatesResult = await _exchangeRateProvider.GetExchangeRates(sourceCurrencies);

            var code = 0;
            exchangeRatesResult.Switch(exchangeRates =>
            {
                PrintExchangeRates(exchangeRates);
            },
            error =>
            {
                PrintValidationError(error);
                code = -1;
            });

            return code;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            _logger.LogError(ex, "Unhandled exception occured.");
            return -2;
        }
    }

    private static void PrintExchangeRates(IEnumerable<ExchangeRate> rates)
    {
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToStringFormat());
        }
    }

    private static void PrintValidationError(Error error)
    {
        Console.WriteLine(error.ToString());
    }
}

