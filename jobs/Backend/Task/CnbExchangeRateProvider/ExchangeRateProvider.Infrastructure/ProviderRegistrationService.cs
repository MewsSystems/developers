using ExchangeRateProvider.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Infrastructure;

/// <summary>
/// Service responsible for registering exchange rate providers with the registry.
/// This follows the Single Responsibility Principle and Dependency Inversion Principle.
/// </summary>
public class ProviderRegistrationService : IProviderRegistrationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProviderRegistry _providerRegistry;

    /// <summary>
    /// Initializes a new instance of the ProviderRegistrationService class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
    /// <param name="providerRegistry">The provider registry to register providers with.</param>
    public ProviderRegistrationService(
        IServiceProvider serviceProvider,
        IProviderRegistry providerRegistry)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _providerRegistry = providerRegistry ?? throw new ArgumentNullException(nameof(providerRegistry));
    }

    /// <summary>
    /// Registers all available providers with the registry.
    /// </summary>
    public void RegisterProviders()
    {
        // Create a scope to resolve scoped services
        using var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        // Resolve and register the CNB provider
        var cnbProvider = scopedProvider.GetRequiredService<CnbExchangeRateProvider>();
        _providerRegistry.RegisterProvider(cnbProvider);
    }
}