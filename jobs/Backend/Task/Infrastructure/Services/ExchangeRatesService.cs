using Application.Abstractions;
using Domain.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ExchangeRatesService : IExchangeRatesService
{
    private readonly CnbApiClient.CnbApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ExchangeRatesService> _logger;

    public ExchangeRatesService(CnbApiClient.CnbApiClient apiClient, IMemoryCache cache, ILogger<ExchangeRatesService> logger)
    {
        _apiClient = apiClient;
        _cache = cache;
        _logger = logger;
    }

    private const string CacheKey = "exchange-rates";

    public async Task<IEnumerable<ExchangeRateDetails>> GetCashedExchangeRatesAsync()
    {
        if (_cache.TryGetValue(CacheKey, out List<ExchangeRateDetails> rates))
        {
            _logger.LogDebug("Loading exchange rates from cache");
        }
        else
        {
            _logger.LogDebug("Exchange rates not found in cache, calling API to get the data");

            var cnbRates = await _apiClient.GetLatestExchangeRatesAsync();
            rates = new List<ExchangeRateDetails>(cnbRates.Rates.Select(r => new ExchangeRateDetails(
                r.Amount,
                r.Country,
                r.Currency,
                r.CurrencyCode,
                r.Rate,
                r.ValidFor)));

            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            _cache.Set(CacheKey, rates, options);
        }

        return rates;
    }
}