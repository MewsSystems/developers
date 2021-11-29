using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.DAL;

public class EndpointDal : IEndpointDal
{
    private readonly ILogger _logger;
    public EndpointDal(ILogger<EndpointDal> logger)
    {
        _logger = logger.NotNull();
    }

    /// <remarks>
    /// Well I know this is not a data access layer implementation
    /// but let`s consider that these data were loaded from database/appsettings/config 
    /// </remarks>
    public IEnumerable<IExchangeRateEndpoint> LoadEndpoints()
    {
        _logger.LogInformation("Creating endpoints from hardcoded configuration");

        var dailyRates = new ExchangeRateEndpoint("CNB Daily Rates",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
            "?date=%DAY%.%MONTH%.%YEAR%");

        var monthlyRates = new ExchangeRateEndpoint("CNB Monthly Rates",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt",
            "?year=%YEAR%&month=%MONTH%");

        return new[] { dailyRates, monthlyRates };
    }
}