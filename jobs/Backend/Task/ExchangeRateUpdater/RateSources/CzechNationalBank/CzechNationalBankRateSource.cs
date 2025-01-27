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
    private readonly TimeProvider _timeProvider;

    public CzechNationalBankRateSource(
        ICzechNationalBankRateParser parser,
        ICzechNationalBankRateUriBuilder uriBuilder,
        IExchangeRateCache cache,
        TimeProvider timeProvider,
        ILogger<CzechNationalBankRateSource> logger,
        IHttpClientFactory httpClientFactory)
    {
        _parser = parser;
        _httpClientFactory = httpClientFactory;
        _uriBuilder = uriBuilder;
        _logger = logger;
        _cache = cache;
        _timeProvider = timeProvider;
    }

    public string SourceName => "CzechNationalBank";
    public async ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var primaryRates = await GetPrimaryRatesAsync(httpClient, targetDate);
        var secondaryRates = await GetSecondaryRatesAsync(httpClient, targetDate);

        // We should also cache primary rates daily, and secondary rates monthly, and get the values from cache if they exist there.
        return primaryRates.Concat(secondaryRates).ToList();
    }

    private async Task<IReadOnlyList<ExchangeRate>> GetPrimaryRatesAsync(HttpClient httpClient, DateOnly targetDate)
    {
        try
        {
            var primaryRatesKey = $"{SourceName}_primary_{targetDate}";

            var uri = _uriBuilder.BuildMainSourceUri(targetDate);
            _logger.LogTrace("Calling CNB api for main source. [Endpoint = {endpoint}]", uri);
            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            return await ParseBankResponse(responseStr);
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
            var secondaryRatesKey = $"{SourceName}_secondary_{targetDate.Year}_{targetDate.Month}";

            var uri = _uriBuilder.BuildSecondarySourceUri(targetDate);
            var response = await httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var responseStr = await response.Content.ReadAsStringAsync();
            return await ParseBankResponse(responseStr);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting secondary rates.");
            return [];
        }
    }

    private async Task<IReadOnlyList<ExchangeRate>> ParseBankResponse(string responseStr)
    {
        var parsedResponse = _parser.Parse(responseStr).ToList();
        return parsedResponse;
    }
}