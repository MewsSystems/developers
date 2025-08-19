using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Services.Builders;

public class ExchangeRateBuilder : IExchangeRateBuilder
{
    private static readonly Currency CzkCurrency = new(CnbConstants.CurrencyCode);

    public IEnumerable<ExchangeRate> BuildExchangeRates(IEnumerable<Currency> requestedCurrencies, CnbExchangeRateData cnbData)
    {
        var exchangeRates = new List<ExchangeRate>();
        var requestedCurrencyCodes = new HashSet<string>(
            requestedCurrencies.Select(c => c.Code),
            StringComparer.OrdinalIgnoreCase);

        foreach (var rate in cnbData.Rates)
        {
            if (requestedCurrencyCodes.Contains(rate.Code) &&
                requestedCurrencyCodes.Contains(CzkCurrency.Code))
            {
                var foreignToCzk = new ExchangeRate(
                    new Currency(rate.Code),
                    CzkCurrency,
                    rate.Rate / rate.Amount
                );
                exchangeRates.Add(foreignToCzk);
            }
        }

        return exchangeRates;
    }
}
