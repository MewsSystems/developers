using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services;

/// <summary>
/// Provides exchange rates from the Czech National Bank with optional caching.
/// </summary>
public class ExchangeRateProvider(
    ICnbApiClient apiClient,
    ICnbDataParser dataParser,
    ILogger<ExchangeRateProvider> logger,
    IOptions<CnbExchangeRateConfiguration> configuration,
    IExchangeRateCache? cache = null,
    ISupportedCurrenciesCache? supportedCurrenciesCache = null)
{
    private readonly ICnbApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    private readonly ICnbDataParser _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
    private readonly IExchangeRateCache? _cache = cache;
    private readonly ISupportedCurrenciesCache? _supportedCurrenciesCache = supportedCurrenciesCache;
    private readonly ILogger<ExchangeRateProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly CnbExchangeRateConfiguration _configuration = (configuration?.Value ?? throw new ArgumentNullException(nameof(configuration)));

    private static readonly Currency CzkCurrency = new("CZK");

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        return GetExchangeRatesAsync(currencies, CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Asynchronously retrieves exchange rates for the specified currencies.
    /// </summary>
    /// <param name="currencies">Currencies to get rates for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Exchange rates as defined by CNB (foreign currency to CZK).</returns>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(currencies);

        var currencyCodes = currencies.Select(c => c.Code).ToList();
        if (currencyCodes.Count == 0)
        {
            _logger.LogInformation(LogMessages.ExchangeRateProvider.NoCurrenciesRequested);
            return Enumerable.Empty<ExchangeRate>();
        }

        _logger.LogInformation(LogMessages.ExchangeRateProvider.FetchingExchangeRates, currencyCodes.Count);

        if (_configuration.EnableCache && _cache != null)
        {
            var cachedRates = _cache.GetCachedRates(currencyCodes);
            if (cachedRates != null)
            {
                _logger.LogInformation(LogMessages.ExchangeRateProvider.ReturningFromCache, cachedRates.Count());
                return cachedRates;
            }
        }

        try
        {
            var rawData = await _apiClient.FetchExchangeRatesAsync(cancellationToken);

            var cnbRates = _dataParser.Parse(rawData);

            var requestedCurrencyCodes = new HashSet<string>(
                currencyCodes,
                StringComparer.OrdinalIgnoreCase);

            var exchangeRates = cnbRates
                .Where(dto => requestedCurrencyCodes.Contains(dto.Code))
                .Select(dto => ConvertToExchangeRate(dto))
                .ToList();

            _logger.LogInformation(
                LogMessages.ExchangeRateProvider.RetrievalSuccessful,
                exchangeRates.Count,
                currencyCodes.Count);

            if (_configuration.EnableCache && _cache != null && exchangeRates.Any())
            {
                _cache.SetCachedRates(exchangeRates);
            }

            return exchangeRates;
        }
        catch (ExchangeRateProviderException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ExchangeRateProvider.UnexpectedError);
            throw new ExchangeRateProviderException(ExceptionMessages.ExchangeRateProvider.FailedToRetrieveRates, ex);
        }
    }

    /// <summary>
    /// Gets all currency codes currently supported by CNB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of supported currency codes.</returns>
    public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        if (_configuration.EnableCache && _supportedCurrenciesCache != null)
        {
            var cached = _supportedCurrenciesCache.GetCachedCurrencies();
            if (cached != null)
            {
                _logger.LogInformation(LogMessages.ExchangeRateProvider.ReturningCachedSupportedCurrencies);
                return cached;
            }
        }

        try
        {
            _logger.LogInformation(LogMessages.ExchangeRateProvider.FetchingSupportedCurrencies);

            var rawData = await _apiClient.FetchExchangeRatesAsync(cancellationToken);
            var cnbRates = _dataParser.Parse(rawData);

            var currencyCodes = cnbRates.Select(dto => dto.Code).OrderBy(c => c).ToList();

            if (_configuration.EnableCache && _supportedCurrenciesCache != null && currencyCodes.Any())
            {
                _supportedCurrenciesCache.SetCachedCurrencies(currencyCodes);
            }

            _logger.LogInformation(LogMessages.ExchangeRateProvider.FoundSupportedCurrencies, currencyCodes.Count);
            return currencyCodes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ExchangeRateProvider.FailedToFetchSupportedCurrencies);
            throw new ExchangeRateProviderException(ExceptionMessages.ExchangeRateProvider.FailedToRetrieveSupportedCurrencies, ex);
        }
    }

    private ExchangeRate ConvertToExchangeRate(CnbExchangeRateDto dto)
    {
        var sourceCurrency = new Currency(dto.Code);
        var rate = dto.Rate / dto.Amount;

        return new ExchangeRate(sourceCurrency, CzkCurrency, rate);
    }
}
