using Common.Interfaces;
using InfrastructureLayer.ExternalServices.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer.ExternalServices.Discovery;

/// <summary>
/// Service that discovers IExchangeRateProvider implementations and wraps them in adapters.
/// </summary>
public class ProviderDiscoveryService : IProviderDiscoveryService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProviderDiscoveryService> _logger;
    private readonly Dictionary<string, IExchangeRateProviderAdapter> _adapters;

    public ProviderDiscoveryService(
        IServiceProvider serviceProvider,
        ILogger<ProviderDiscoveryService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _adapters = new Dictionary<string, IExchangeRateProviderAdapter>(StringComparer.OrdinalIgnoreCase);

        InitializeAdapters();
    }

    private void InitializeAdapters()
    {
        try
        {
            // Get all registered IExchangeRateProvider implementations
            var providers = _serviceProvider.GetServices<IExchangeRateProvider>().ToList();

            _logger.LogInformation(
                "Discovered {Count} exchange rate providers",
                providers.Count);

            // Create adapters for each provider
            // The provider code should match the database configuration
            foreach (var provider in providers)
            {
                var providerType = provider.GetType();
                string providerCode = DetermineProviderCode(providerType);

                var adapter = new ExchangeRateProviderAdapter(
                    provider,
                    providerCode,
                    _serviceProvider.GetRequiredService<ILogger<ExchangeRateProviderAdapter>>());

                _adapters[providerCode] = adapter;

                _logger.LogInformation(
                    "Registered adapter for provider {ProviderCode} (Type: {ProviderType})",
                    providerCode,
                    providerType.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing provider adapters");
        }
    }

    private string DetermineProviderCode(Type providerType)
    {
        // Map provider type names to codes
        var typeName = providerType.Name;

        return typeName switch
        {
            "EuropeanCentralBankProvider" => "ECB",
            "CzechNationalBankProvider" => "CNB",
            "RomanianNationalBankProvider" => "BNR",
            _ => typeName.Replace("Provider", "").ToUpperInvariant()
        };
    }

    public IEnumerable<IExchangeRateProviderAdapter> DiscoverProviders()
    {
        return _adapters.Values;
    }

    public IExchangeRateProviderAdapter? GetProviderByCode(string providerCode)
    {
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            _logger.LogWarning("Provider code is null or empty");
            return null;
        }

        if (_adapters.TryGetValue(providerCode, out var adapter))
        {
            return adapter;
        }

        _logger.LogWarning("Provider with code '{ProviderCode}' not found", providerCode);
        return null;
    }

    public IEnumerable<string> GetRegisteredProviderCodes()
    {
        return _adapters.Keys;
    }
}
