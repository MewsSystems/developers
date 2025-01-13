using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider(
    IExchangeRateService exchangeRateService,
    ILogger<ExchangeRateProvider> logger)
{
    public async Task<List<ExchangeRate>> GetExchangeRates(
        DateTime date,
        Currency targetCurrency,
        IEnumerable<Currency> currencies)
    {
        var response = await exchangeRateService.GetExchangeRatesAsync(date);

        if (response.Rates.Count is 0)
        {
            logger.LogInformation("No exchange rates for were found for {date}", date.ToShortDateString());
            return new List<ExchangeRate>();
        }

        var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code));

        var exchangeRates = response.Rates
            .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
            .Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode, rate.Currency),
                targetCurrency,
                rate.Rate / rate.Amount,
                rate.ValidFor))
            .ToList();

        if (exchangeRates.Count is 0)
        {
            logger.LogInformation("No exchange rates for specified currencies were found for {date}", date.ToShortDateString());
            return exchangeRates;
        }

        logger.LogInformation("{count} exchange rates found for specified currencies", exchangeRates.Count);

        return exchangeRates;
    }
}