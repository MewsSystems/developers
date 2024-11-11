using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

namespace Mews.ExchangeRateUpdater.Application.Interfaces;

/// <summary>
/// Exchange rates repository definition.
/// </summary>
public interface IExchangeRateRepository
{
    /// <summary>
    /// Gets a collection of today's exchange rates.
    /// </summary>
    /// <returns>List of <see cref="ExchangeRate"/></returns>
    Task<IEnumerable<ExchangeRate>> GetTodayExchangeRatesAsync();

    /// <summary>
    /// Gets a cached a collection of today's exchange rates if it exists,
    /// else it goes to the data source.
    /// </summary>
    /// <returns>List of <see cref="ExchangeRate"/></returns>
    Task<IEnumerable<ExchangeRate>> GetCachedTodayExchangeRatesAsync();
}
