namespace ExchangeRateProvider.Application.Interfaces;

using Domain.Entities;

/// <summary>
///     Provider interface for fetching exchange rates from external sources.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    ///     Retrieves all available exchange rates from the external data source.
    /// </summary>
    /// <returns>A collection of exchange rates.</returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
}
