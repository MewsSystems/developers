using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProviders;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateDataProviderFactory _dataProviderFactory;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(IExchangeRateDataProviderFactory dataProviderFactory, ILogger<ExchangeRateService> logger)
    {
		_dataProviderFactory = dataProviderFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(string TargetCurrency, IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        if (currencies == null)
        {
            _logger.LogWarning("Requested currencies collection is null. Returning empty result.");
            return Enumerable.Empty<ExchangeRate>();
        }

        var requestedCurrencies = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

		CurrencyValidator.ValidateCurrencyCodes(currencies);

		_logger.LogDebug("Fetching exchange rates for {Count} requested currencies via provider {ProviderCurrency}.", requestedCurrencies.Count, TargetCurrency);

		var provider = _dataProviderFactory.GetProvider(TargetCurrency);
		var allRates = await provider.GetDailyRatesAsync(cancellationToken);
        var requestedCurrenciesRates = allRates.Where(r => requestedCurrencies.Contains(r.SourceCurrency.Code)).ToList();

		_logger.LogInformation("Provider {ProviderCurrency} returned {Filtered}/{Total} matching rates.", provider.ExchangeRateProviderTargetCurrencyCode, requestedCurrenciesRates.Count, allRates.Count());
		return requestedCurrenciesRates;
    }
}
