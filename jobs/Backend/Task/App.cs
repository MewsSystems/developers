using System;
using System.Linq;
using ExchangeRateUpdater.DAL;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

internal class App
{
    private readonly ILogger<App> _logger;
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly IEndpointDal _endpointDal;

    public App(ILogger<App> logger, IExchangeRateProvider exchangeRateProvider, IEndpointDal endpointDal)
    {
        _logger = logger.NotNull();
        _exchangeRateProvider = exchangeRateProvider.NotNull();
        _endpointDal = endpointDal.NotNull();
    }

    public void Run()
    {
        var currencies = new[]
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

        try
        {
            var endpoints = _endpointDal.LoadEndpoints();
            _exchangeRateProvider.SetEndpoints(endpoints);
            _exchangeRateProvider.FetchRates();
            var rates = _exchangeRateProvider.GetExchangeRates(currencies).ToList();
                
            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}