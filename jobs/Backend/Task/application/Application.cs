using System;
using ExchangeRateUpdater.config;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.application;

public class Application
{
    private readonly AppConfiguration _appConfiguration;
    private readonly ILogger<Application> _logger;

    public Application(ILogger<Application> logger, AppConfiguration appConfiguration)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
    }

    public void Run()
    {
        try
        {
            _logger.LogInformation("Starting exchange rate retrieval for currencies: {Currencies}",
                _appConfiguration.GetCurrencies());

            // var provider = new ExchangeRateProvider();
            // var rates = provider.GetExchangeRates(currencies);
            //
            // Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            // foreach (var rate in rates)
            // {
            //     Console.WriteLine(rate.ToString());
            // }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve exchange rates");
        }
    }
}