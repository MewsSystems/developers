using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.CzechCrown.Models;
using Services.CzechCrown.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Services.CzechCrown;

internal class CzechNationalBankCachedClient : ICzechNationalBankClient
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ICzechNationalBankClient> _logger;
    private readonly CzechNationalBankClientOptions _options;

    private const string CacheKey = nameof(CzechNationalBankCachedClient);

    public CzechNationalBankCachedClient(HttpClient httpClient, IDistributedCache distributedCache, IOptions<CzechNationalBankClientOptions> options, ILogger<CzechNationalBankCachedClient> logger)
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
