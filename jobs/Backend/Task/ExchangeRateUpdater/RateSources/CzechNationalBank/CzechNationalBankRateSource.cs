using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cache;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankRateSource : IRateSource
{
    private readonly ICzechNationalBankRateParser _parser;
    private readonly ICzechNationalBankRateUriBuilder _uriBuilder;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CzechNationalBankRateSource> _logger;
    private readonly IExchangeRateCache _cache;
    private readonly ICzechNationalBankRatesCacheExpirationCalculator _cacheExpirationCalculator;

    public CzechNationalBankRateSource(
        ICzechNationalBankRateParser parser,
        ICzechNationalBankRateUriBuilder uriBuilder,
        IExchangeRateCache cache,
        ICzechNationalBankRatesCacheExpirationCalculator cacheExpirationCalculator,
        TimeProvider timeProvider,
        ILogger<CzechNationalBankRateSource> logger,
        IHttpClientFactory httpClientFactory)
    {
        _parser = parser;
        _httpClientFactory = httpClientFactory;
        _uriBuilder = uriBuilder;
        _logger = logger;
        _cache = cache;
        _cacheExpirationCalculator = cacheExpirationCalculator;
    }

    public string SourceName => "CzechNationalBank";
    public async ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var primaryRatesKey = CzechNationalBankCacheKeyBuilder.PrimaryCacheKey(targetDate);
        var secondaryRatesKey = CzechNationalBankCacheKeyBuilder.SecondaryCacheKey(targetDate);

        var primaryRates = await _cache.GetOrCreateAsync(
            primaryRatesKey,
            async () => await GetPrimaryRatesAsync(httpClient, targetDate),
            () => _cacheExpirationCalculator.GetPrimaryRateExpirationDate(targetDate));
        
        var secondaryRates = await _cache.GetOrCreateAsync(secondaryRatesKey,
            async () => await GetSecondaryRatesAsync(httpClient, targetDate),
            () => _cacheExpirationCalculator.GetSecondaryRateExpirationDate(targetDate));

        return primaryRates.Concat(secondaryRates).ToList();
    }

    private async Task<IReadOnlyList<ExchangeRate>> GetPrimaryRatesAsync(HttpClient httpClient, DateOnly targetDate)
    {
        try
        {
            var uri = _uriBuilder.BuildMainSourceUri(targetDate);
            _logger.LogTrace("Calling CNB api for main source. [Endpoint = {endpoint}]", uri);
            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            return ParseBankResponse(responseStr);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting secondary rates.");
            return [];
        }
    }


    private async Task<IReadOnlyList<ExchangeRate>> GetSecondaryRatesAsync(HttpClient httpClient, DateOnly targetDate)
    {
        try
        {
            var uri = _uriBuilder.BuildSecondarySourceUri(targetDate);
            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            return ParseBankResponse(responseStr);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting secondary rates.");
            return [];
        }
    }

    private IReadOnlyList<ExchangeRate> ParseBankResponse(string responseStr)
    {
        var parsedResponse = _parser.Parse(responseStr).ToList();
        return parsedResponse;
    }
}