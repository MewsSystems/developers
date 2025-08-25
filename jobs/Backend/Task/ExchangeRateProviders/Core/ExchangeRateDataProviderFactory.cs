using Microsoft.Extensions.Logging;

namespace ExchangeRateProviders.Core;

public class ExchangeRateDataProviderFactory : IExchangeRateDataProviderFactory
{
    private readonly Dictionary<string, IExchangeRateDataProvider> _providers;
    private readonly ILogger<ExchangeRateDataProviderFactory> _logger;

    public ExchangeRateDataProviderFactory(IEnumerable<IExchangeRateDataProvider> providers, ILogger<ExchangeRateDataProviderFactory> logger)
    {
        _logger = logger;
		_providers = providers.ToDictionary(p => p.ExchangeRateProviderTargetCurrencyCode, StringComparer.OrdinalIgnoreCase);
    }

    public IExchangeRateDataProvider GetProvider(string exchangeRateProviderCurrencyCode)
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
