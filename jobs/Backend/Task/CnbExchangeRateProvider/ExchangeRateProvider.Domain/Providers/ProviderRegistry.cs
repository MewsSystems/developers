using ExchangeRateProvider.Domain.Entities;
using ExchangeRateProvider.Domain.Interfaces;

namespace ExchangeRateProvider.Domain.Providers;

/// <summary>
/// Default implementation of the provider registry.
/// </summary>
public class ProviderRegistry : IProviderRegistry
{
    private readonly List<IExchangeRateProvider> _providers = new();
    private readonly object _lock = new();

    /// <inheritdoc />
    public void RegisterProvider(IExchangeRateProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        lock (_lock)
        {
            if (!_providers.Contains(provider))
            {
                _providers.Add(provider);
                // Sort by priority (higher priority first)
                _providers.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            }
        }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IExchangeRateProvider> GetAllProviders()
    {
        lock (_lock)
        {
            return _providers.ToArray();
        }
    }

    /// <inheritdoc />
    public IExchangeRateProvider? GetProviderFor(IEnumerable<Currency> currencies)
    {
        return GetProvidersFor(currencies).FirstOrDefault();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IExchangeRateProvider> GetProvidersFor(IEnumerable<Currency> currencies)
    {
        lock (_lock)
        {
            return _providers
                .Where(p => p.CanHandle(currencies))
                .ToArray();
        }
    }
}
