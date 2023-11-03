using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Cnb;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly ICnbClient _cnbClient;

    public ExchangeRateProvider(ICnbClient cnbClient)
    {
        ArgumentNullException.ThrowIfNull(cnbClient);
        _cnbClient = cnbClient;
    }
    
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var ratesPayload = _cnbClient.GetCurrentExchangeRates().GetAwaiter().GetResult();

        return ratesPayload.Rates
            .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
            .Select(
                r => new ExchangeRate(
                    sourceCurrency: new Currency(r.CurrencyCode),
                    targetCurrency: new Currency("CZK"),
                    value: r.ExchangeRate / r.Amount));
    }
}


