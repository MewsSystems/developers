using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Infrastructure.HttpClients;

/// <summary>
/// Czech National Bank exchange rates API client definition.
/// </summary>
public interface ICnbApiClient
{
    /// <summary>
    /// Gets a collection of today's exchange rates.
    /// </summary>
    /// <returns><see cref="CnbExchangeRates"/></returns>
    Task<CnbExchangeRates> GetTodayExchangeRatesAsync();

    /// <summary>
    /// Gets a collection of exchange rates for a given date.
    /// </summary>
    /// <param name="date">The date to retrieve the exchange rates</param>
    /// <returns><see cref="CnbExchangeRates"/></returns>
    Task<CnbExchangeRates> GetExchangeRatesAsync(DateTime date);
}
