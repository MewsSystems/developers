using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Domain.Interfaces;

/// <summary>
/// Provides exchange rates from an external source.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Retrieves the latest available exchange rates.
    /// </summary>
    /// <returns>
    /// A collection of exchange rates indexed by currency code.
    /// </returns>
    /// <exception cref="ExchangeRateException">
    /// Thrown when exchange rates cannot be retrieved.
    /// </exception>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);
}