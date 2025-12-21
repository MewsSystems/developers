using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Application.Interfaces;

/// <summary>
/// Defines the contract for the exchange rate provider service that orchestrates the retrieval of
/// exchange rates for specific currencies.
/// </summary>
public interface IExchangeRateProviderService
{
    /// <summary>
    /// Retrieves the latest exchange rates for the specified currencies from available providers.
    /// </summary>
    /// <param name="currencies">The collection of currencies for which to retrieve exchange rates.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only
    /// list of exchange rates for the specified currencies.</returns>
    Task<IReadOnlyList<ExchangeRate>> GetLatestAsync(IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);
}
