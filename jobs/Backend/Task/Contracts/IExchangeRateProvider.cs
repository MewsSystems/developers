using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateProvider
{
    /// <summary>
    /// Returns exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source. It does not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// it does not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, it will be ignored.
    /// </summary>
    /// <param name="currencies">Source currencies whose exchange rate is needed.</param>
    /// <param name="date">The date for which exchange rates are requested (null for the latest rates).</param>
    /// <returns>
    /// Returns a collection of ExchangeRate objects containing the requested information.
    /// </returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}
