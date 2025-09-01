using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Domain.Interfaces;

/// <summary>
/// Registry for managing exchange rate providers.
/// </summary>
public interface IProviderRegistry
{
    /// <summary>
    /// Registers a provider with the registry.
    /// </summary>
    /// <param name="provider">The provider to register.</param>
    void RegisterProvider(IExchangeRateProvider provider);

    /// <summary>
    /// Gets all registered providers.
    /// </summary>
    IReadOnlyCollection<IExchangeRateProvider> GetAllProviders();

    /// <summary>
    /// Gets the best provider for the specified currencies.
    /// </summary>
    /// <param name="currencies">The currencies to handle.</param>
    /// <returns>The best provider, or null if none can handle the currencies.</returns>
    IExchangeRateProvider? GetProviderFor(IEnumerable<Currency> currencies);

    /// <summary>
    /// Gets all providers that can handle the specified currencies.
    /// </summary>
    /// <param name="currencies">The currencies to handle.</param>
    /// <returns>A collection of providers that can handle the currencies.</returns>
    IReadOnlyCollection<IExchangeRateProvider> GetProvidersFor(IEnumerable<Currency> currencies);
}

/// <summary>
/// Service for initializing provider registrations.
/// </summary>
public interface IProviderRegistrationService
{
    /// <summary>
    /// Registers all available providers with the registry.
    /// </summary>
    void RegisterProviders();
}

/// <summary>
/// Configuration for provider registration.
/// </summary>
public class ProviderConfiguration
{
    /// <summary>
    /// Gets or sets the list of provider types to register.
    /// </summary>
    public List<Type> ProviderTypes { get; set; } = new();

    /// <summary>
    /// Gets or sets whether to enable the default CNB provider.
    /// </summary>
    public bool EnableCnbProvider { get; set; } = true;
}