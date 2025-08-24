using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk.Config;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProviders.Czk;

public class CzkExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateDataProvider _dataProvider;
    private readonly ILogger<CzkExchangeRateProvider> _logger;

    public CzkExchangeRateProvider(IExchangeRateDataProvider dataProvider, ILogger<CzkExchangeRateProvider> logger)
    {
        _dataProvider = dataProvider;
        _logger = logger;
    }

    public string ExchangeRateProviderCurrencyCode => Constants.ExchangeRateProviderCurrencyCode;

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        if (currencies == null)
        {
            _logger.LogWarning("Requested currencies collection is null. Returning empty result.");
            return Enumerable.Empty<ExchangeRate>();
        }

        var requestedCurrencies = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
        _logger.LogDebug("Fetching exchange rates for {Count} requested currencies via provider {ProviderCurrency}.", requestedCurrencies.Count, ExchangeRateProviderCurrencyCode);

        var allRates = await _dataProvider.GetDailyRatesAsync(cancellationToken);
        var requestedCurrenciesRates = allRates.Where(r => requestedCurrencies.Contains(r.SourceCurrency.Code)).ToList();

        _logger.LogInformation("Provider {ProviderCurrency} returned {Filtered}/{Total} matching rates.", ExchangeRateProviderCurrencyCode, requestedCurrenciesRates.Count, allRates.Count());
        return requestedCurrenciesRates;
    }
}
