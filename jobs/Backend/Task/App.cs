using ExchangeRateUpdater.Models.Behavior;
using ExchangeRateUpdater.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ExchangeRateUpdater;

internal class App
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger, IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _logger = logger;
    }

    internal void Run(string[] args)
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var sourceCurrencies = _exchangeRateRepository.GetSourceCurrencies();
            var rates = provider.GetExchangeRates(sourceCurrencies);

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

