namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Interface for caching supported currency codes.
/// </summary>
public interface ISupportedCurrenciesCache
{
    /// <summary>
    /// Gets cached list of supported currency codes.
    /// </summary>
    /// <returns>Cached currency codes, or null if not cached.</returns>
    IEnumerable<string>? GetCachedCurrencies();

    /// <summary>
    /// Caches the list of supported currency codes.
    /// </summary>
    /// <param name="currencyCodes">Currency codes to cache.</param>
    void SetCachedCurrencies(IEnumerable<string> currencyCodes);

    /// <summary>
    /// Clears the cached currency codes.
    /// </summary>
    void Clear();
}
