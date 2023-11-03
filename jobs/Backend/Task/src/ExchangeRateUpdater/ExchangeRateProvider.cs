using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        using var httpClient = new HttpClient();
        var cnbClient = new CnbClient(httpClient, NullLogger<CnbClient>.Instance);

        var ratesPayload = cnbClient.GetCurrentExchangeRates().GetAwaiter().GetResult();

        return ratesPayload.Rates
            .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
            .Select(
                r => new ExchangeRate(
                    sourceCurrency: new Currency(r.CurrencyCode),
                    targetCurrency: new Currency("CZK"),
                    value: r.ExchangeRate / r.Amount));
    }
}


