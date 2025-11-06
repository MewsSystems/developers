using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services;

/// <summary>
/// Provides exchange rates from the Czech National Bank with optional caching.
/// </summary>
public class ExchangeRateProvider
{
    private readonly ICnbApiClient _apiClient;
    private readonly ICnbDataParser _dataParser;
    private readonly IExchangeRateCache? _cache;
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly CnbExchangeRateConfiguration _configuration;

    // CNB provides rates as foreign currency/CZK
    private static readonly Currency CzkCurrency = new("CZK");

    public ExchangeRateProvider(
        ICnbApiClient apiClient,
        ICnbDataParser dataParser,
        ILogger<ExchangeRateProvider> logger,
        IOptions<CnbExchangeRateConfiguration> configuration,
        IExchangeRateCache? cache = null)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        _cache = cache; // Cache is optional
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        // Use async wrapper for backward compatibility with the interface
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
        if (currencies == null)
        {
            throw new ArgumentNullException(nameof(currencies));
        }

        var currencyList = currencies.ToList();
        if (!currencyList.Any())
        {
            _logger.LogInformation("No currencies requested");
            return Enumerable.Empty<ExchangeRate>();
        }

        var currencyCodes = currencyList.Select(c => c.Code).ToList();

        _logger.LogInformation("Fetching exchange rates for {Count} currencies", currencyList.Count);

        // Check cache if enabled
        if (_configuration.EnableCache && _cache != null)
        {
            var cachedRates = _cache.GetCachedRates(currencyCodes);
            if (cachedRates != null)
            {
                _logger.LogInformation("Returning {Count} exchange rates from cache", cachedRates.Count());
                return cachedRates;
            }
        }

        try
        {
            // Fetch raw data from CNB
            var rawData = await _apiClient.FetchExchangeRatesAsync(cancellationToken);

            // Parse the data
            var cnbRates = _dataParser.Parse(rawData);

            // Convert to ExchangeRate objects, filtering for requested currencies
            var requestedCurrencyCodes = new HashSet<string>(
                currencyCodes,
                StringComparer.OrdinalIgnoreCase);

            var exchangeRates = cnbRates
                .Where(dto => requestedCurrencyCodes.Contains(dto.Code))
                .Select(dto => ConvertToExchangeRate(dto))
                .ToList();

            _logger.LogInformation(
                "Successfully retrieved {Retrieved} exchange rates out of {Requested} requested currencies",
                exchangeRates.Count,
                currencyList.Count);

            // Cache the results if enabled
            if (_configuration.EnableCache && _cache != null && exchangeRates.Any())
            {
                _cache.SetCachedRates(exchangeRates);
            }

            return exchangeRates;
        }
        catch (ExchangeRateProviderException)
        {
            // Re-throw provider exceptions as-is
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting exchange rates");
            throw new ExchangeRateProviderException("Failed to retrieve exchange rates", ex);
        }
    }

    private ExchangeRate ConvertToExchangeRate(CnbExchangeRateDto dto)
    {
        // CNB provides rates as: Amount of foreign currency = Rate in CZK
        // Example: 1 USD = 22.950 CZK
        // So the rate represents: source (foreign) / target (CZK) = rate
        var sourceCurrency = new Currency(dto.Code);
        var rate = dto.Rate / dto.Amount; // Normalize to rate per 1 unit of currency

        return new ExchangeRate(sourceCurrency, CzkCurrency, rate);
    }
}

