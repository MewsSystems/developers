using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using ExchangeRateUpdater.DTOs;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Providers;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(IExchangeRateService exchangeRateService, ILogger<ExchangeRateProvider> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    /// <inheritdoc />
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var exchangeRates = _exchangeRateService.GetExchangeRatesAsync().Result;

        var filteredRates = exchangeRates.Rates
            .Where(rate =>
                currencies.Any(currency => currency.Code == rate.CurrencyCode))
            .Select(rate =>
                new ExchangeRate(new Currency(rate.CurrencyCode), new Currency("CZK"), rate.Rate / rate.Amount));

        if (filteredRates.Count() is 0)
        {
            _logger.LogInformation("No matching rates found for chosen currency codes");
        }

        return filteredRates;
    }
}
