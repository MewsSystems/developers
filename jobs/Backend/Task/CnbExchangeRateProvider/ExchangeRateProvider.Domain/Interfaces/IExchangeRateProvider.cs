using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Domain.Interfaces;

/// <summary>
/// Defines the contract for exchange rate providers.
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Gets the name/identifier of this provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the priority of this provider (higher values = higher priority).
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Determines whether this provider can handle the specified currencies.
    /// </summary>
    /// <param name="currencies">The currencies to check.</param>
    /// <returns>True if this provider can handle the currencies, false otherwise.</returns>
    bool CanHandle(IEnumerable<Currency> currencies);

    /// <summary>
    /// Gets exchange rates for the specified currencies.
    /// </summary>
    /// <param name="currencies">The currencies to get rates for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of exchange rates.</returns>
    Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default);
}
