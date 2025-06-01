using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Models.CzechNationalBankApi;
using Services.Options;

namespace Services;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken ct = default);
}

internal class CzechCrownRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankClient _czechNationalBankClient;
    private readonly ILogger<CzechCrownRateProvider> _logger;

    public CzechCrownRateProvider(ICzechNationalBankClient czechNationalBankClient, ILogger<CzechCrownRateProvider> logger)
    {
        _czechNationalBankClient = czechNationalBankClient;
        _logger = logger;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken ct = default)
    {
        var currencyList = currencies.Distinct().ToList();
        var executionDate = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogInformation("Getting exchange rates for {CurrencyCount} currencies (after deduplication)", currencyList.Count);
        _logger.LogDebug("Getting exchange rates for {Currencies}", string.Join(", ", currencyList.Select(c => c.Code)));

        var clientResponse = await _czechNationalBankClient.GetExchangeRates(executionDate, ct);
        var primaryResults = GetRatesFromClientResponse(currencyList, clientResponse, executionDate);

        if (primaryResults.Count == currencyList.Count)
        {
            _logger.LogInformation("Execution complete");
            _logger.LogDebug("All requested currencies found in primary response");
            return primaryResults.Order();
        }

        if (ct.IsCancellationRequested)
        {
            _logger.LogDebug("Operation cancelled, returning empty list");
            return [];
        }

        var remainingCurrencies = currencyList.Except(primaryResults.Select(r => r.SourceCurrency)).ToList();

        var secondaryClientResponse = await _czechNationalBankClient.GetOtherExchangeRates(executionDate, ct);
        var secondaryResults = GetRatesFromClientResponse(remainingCurrencies, secondaryClientResponse, executionDate);

        var results = primaryResults.Concat(secondaryResults).Order();

        _logger.LogInformation("Execution complete");
        _logger.LogDebug("Found {ResultCount} exchange rates in total for {CurrencyCount} requested currencies", results.Count(), currencyList.Count);
        
        return results;
    }

    private List<ExchangeRate> GetRatesFromClientResponse(IEnumerable<Currency> currencies, CzkExchangeRateResponse clientResponse, DateOnly date)
    {
        List<ExchangeRate> czechCrownExchangeRates = [];
        foreach (var currency in currencies)
        {
            var rate = clientResponse.Rates.Where(r => string.Equals(r.CurrencyCode, currency.Code, StringComparison.OrdinalIgnoreCase))
                .MaxBy(r => r.ValidFor);
            if (rate == null)
            {
                _logger.LogDebug("Currency {Currency} not found in response", currency.Code);
                continue;
            }

            if (rate.ValidFor != date || rate.ValidFor > date)
            {
                _logger.LogWarning("Rate for {Currency} is not for the requested date {Date}, using latest available rate (from {RateDate})", currency.Code, date, rate.ValidFor);
            }

            var rateValue = rate.Rate / rate.Amount;
            _logger.LogDebug("Found exchange rate of {Date} for {Currency}: {RateValue}", rate.ValidFor, currency.Code, rateValue);
            czechCrownExchangeRates.Add(new ExchangeRate(new Currency(currency.Code), Currency.Czk, rateValue));
        }
        return czechCrownExchangeRates;
    }
}

public interface ICzechNationalBankClient
{
    Task<CzkExchangeRateResponse> GetExchangeRates(DateOnly date, CancellationToken ct = default);
    Task<CzkExchangeRateResponse> GetOtherExchangeRates(DateOnly date, CancellationToken ct = default);
}

internal class CzechNationalBankCachedClient : ICzechNationalBankClient
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ICzechNationalBankClient> _logger;
    private readonly CzechNationalBankClientOptions _options;

    private const string CacheKey = nameof(CzechNationalBankCachedClient);

    public CzechNationalBankCachedClient(HttpClient httpClient, IDistributedCache distributedCache, IOptions<CzechNationalBankClientOptions> options, ILogger<ICzechNationalBankClient> logger)
    {
        _httpClient = httpClient;
        _distributedCache = distributedCache;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<CzkExchangeRateResponse> GetExchangeRates(DateOnly date, CancellationToken ct = default)
    {
        var dateParameter = $"{date:yyyy-MM-dd}";
        var endpoint = $"{_options.ExchangeRatesEndpoint}?date={dateParameter}";
        var cacheKey = $"{CacheKey}ExchangeRates{dateParameter}";
        return await GetCachedValueOrCallApi(endpoint, date, cacheKey, ct);
    }

    public async Task<CzkExchangeRateResponse> GetOtherExchangeRates(DateOnly date, CancellationToken ct = default)
    {
        var dateParameter = $"{date:yyyy-MM}";
        var endpoint = $"{_options.ForexRatesEndpoint}?yearMonth={dateParameter}";
        var cacheKey = $"{CacheKey}ForeignRates{dateParameter}";
        return await GetCachedValueOrCallApi(endpoint, date, cacheKey, ct);
    }

    private async Task<CzkExchangeRateResponse> GetCachedValueOrCallApi(string endpoint, DateOnly date, string cacheKey, CancellationToken ct)
    {
        var cachedResponse = await GetCachedResponse(cacheKey, ct);
        if (cachedResponse != null)
        {
            _logger.LogDebug("Returning cached response for endpoint {Endpoint}, cache key {CacheKey}", endpoint, cacheKey);
            return cachedResponse;
        }

        if (ct.IsCancellationRequested)
        {
            _logger.LogDebug("Operation cancelled, returning empty response");
            return CzkExchangeRateResponse.Empty;
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<CzkExchangeRateResponse>(endpoint, ct);
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), ct);
            _logger.LogInformation("Retrieved exchange rates for date {Date} from endpoint {Endpoint}", date, endpoint);
            return response ?? CzkExchangeRateResponse.Empty;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to retrieve exchange rates from endpoint {Endpoint}", endpoint);
            return CzkExchangeRateResponse.Empty;
        }
    }

    private async Task<CzkExchangeRateResponse?> GetCachedResponse(string cacheKey, CancellationToken ct)
    {
        var cachedStringResponse = await _distributedCache.GetStringAsync(cacheKey, ct);
        if (cachedStringResponse != null)
        {
            try
            {
                return JsonSerializer.Deserialize<CzkExchangeRateResponse>(cachedStringResponse);
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Failed to deserialize cached response for key {CacheKey}: raw content was {RawContent}", cacheKey, cachedStringResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while retrieving cached response for key {CacheKey}", cacheKey);
            }
        }
        return null;
    }
}