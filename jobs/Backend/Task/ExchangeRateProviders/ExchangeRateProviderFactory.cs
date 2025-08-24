using ExchangeRateProviders.Core;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProviders;

public class ExchangeRateProviderFactory : IExchangeRateProviderFactory
{
    private readonly Dictionary<string, IExchangeRateProvider> _providers;
    private readonly ILogger<ExchangeRateProviderFactory> _logger;

    public ExchangeRateProviderFactory(IEnumerable<IExchangeRateProvider> providers, ILogger<ExchangeRateProviderFactory> logger)
    {
        _logger = logger;
		_providers = providers.ToDictionary(p => p.ExchangeRateProviderCurrencyCode, StringComparer.OrdinalIgnoreCase);
    }

    public IExchangeRateProvider GetProvider(string exchangeRateProviderCurrencyCode)
    {
        if (exchangeRateProviderCurrencyCode == null)
        {
            _logger.LogError("Attempted to get provider with null currency code.");
            throw new ArgumentNullException(nameof(exchangeRateProviderCurrencyCode));
        }
        if (_providers.TryGetValue(exchangeRateProviderCurrencyCode, out var provider))
        {
            _logger.LogDebug("Resolved exchange rate provider for currency {Currency}", exchangeRateProviderCurrencyCode);
            return provider;
        }
        _logger.LogError("No exchange rate provider registered for currency {Currency}", exchangeRateProviderCurrencyCode);
        throw new InvalidOperationException($"No exchange rate provider registered for currency '{exchangeRateProviderCurrencyCode}'.");
    }
}
