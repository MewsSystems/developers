using InfrastructureLayer.ExternalServices.Adapters;

namespace InfrastructureLayer.ExternalServices.Discovery;

/// <summary>
/// Service for discovering and accessing registered exchange rate provider adapters.
/// </summary>
public interface IProviderDiscoveryService
{
    /// <summary>
    /// Discovers all registered IExchangeRateProvider implementations (wrapped in adapters).
    /// </summary>
    IEnumerable<IExchangeRateProviderAdapter> DiscoverProviders();

    /// <summary>
    /// Gets a specific provider adapter by its code.
    /// </summary>
    /// <param name="providerCode">The unique provider code (e.g., "ECB", "CNB", "BNR")</param>
    /// <returns>The provider adapter, or null if not found</returns>
    IExchangeRateProviderAdapter? GetProviderByCode(string providerCode);

    /// <summary>
    /// Gets all provider codes that are currently registered.
    /// </summary>
    IEnumerable<string> GetRegisteredProviderCodes();
}
