using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions;

/// <summary>
/// Provides normalized currency rates for the specified currencies, relative to a common base.
/// Only the available requested currencies are included (unavailable and duplicates are filtered out).
/// </summary>
public interface IExchangeRateDataProvider
{
    /// <summary>
    /// Retrieves normalized exchange rates for the specified currencies.
    /// The returned dictionary uses the uppercase currency code as the key and a tuple containing the Currency
    /// instance and its normalized rate as the value.
    /// </summary>
    /// <param name="currencies">The collection of currencies for which to retrieve exchange rates.</param>
    /// <returns>
    /// An IReadOnlyDictionary mapping a currency code to a tuple containing:
    /// - Currency: the Currency instance,
    /// - Rate: the normalized exchange rate (e.g. rate divided by amount).
    /// </returns>
    Task<IReadOnlyDictionary<string, (Currency Currency, decimal Rate)>> GetNormalizedRatesAsync(IEnumerable<Currency> currencies);
}
