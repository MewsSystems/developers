using ConfigurationLayer.Option;

namespace ConfigurationLayer.Interface;

/// <summary>
/// Service for loading exchange rate provider configurations.
/// Implements database-first pattern with appsettings.json fallback.
/// </summary>
public interface IProviderConfigurationService
{
    /// <summary>
    /// Loads provider configuration by provider code.
    /// Priority: Database → appsettings.json → null
    /// </summary>
    /// <param name="providerCode">The provider code (e.g., "BNR", "CNB", "ECB")</param>
    /// <returns>Provider configuration if found, null otherwise</returns>
    Task<ExchangeRateProviderOptions?> GetProviderConfigurationAsync(string providerCode);

    /// <summary>
    /// Loads all active provider configurations.
    /// Priority: Database → appsettings.json
    /// </summary>
    /// <returns>List of all active provider configurations</returns>
    Task<List<ExchangeRateProviderOptions>> GetAllActiveProviderConfigurationsAsync();

    /// <summary>
    /// Refreshes the cached provider configurations.
    /// Call this after updating provider configuration in the database.
    /// </summary>
    Task RefreshCacheAsync();
}
