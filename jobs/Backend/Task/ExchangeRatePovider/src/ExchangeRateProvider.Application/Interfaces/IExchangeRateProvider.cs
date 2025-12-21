using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Application.Interfaces;

/// <summary>
/// Defines the contract for exchange rate providers that can retrieve exchange rate data
/// from external sources.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Retrieves the latest exchange rates from the external data source.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// a read-only list of exchange rates.</returns>
    Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(CancellationToken cancellationToken = default);
}
