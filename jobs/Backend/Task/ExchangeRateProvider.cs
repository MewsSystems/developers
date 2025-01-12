using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider(IExchangeRateService exchangeRateService)
{
    public async Task<List<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime date)
    {
        var response = await exchangeRateService.GetExchangeRatesAsync(date);

        var currencyCodes = currencies.Select(c => c.Code);

        var targetCurrency = new Currency("CZK", "koruna");

        var exchangeRates = response.Rates
            .Where(rate => currencyCodes.Contains(rate.CurrencyCode))
            .Select(rate => new ExchangeRate(
                new Currency(rate.CurrencyCode, rate.Currency), 
                targetCurrency, 
                rate.Rate / rate.Amount))
            .ToList();

        return exchangeRates;
    }
}