namespace ExchangeRateProvider.Infrastructure.Services;

using Application.Interfaces;
using Domain.Entities;
using Domain.Options;
using Infrasctructure.Clients;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class CnbExchangeRateProvider(
    CnbCzClient cnbCzClient,
    IOptions<CnbApiOptions> options,
    ILogger<CnbExchangeRateProvider> logger,
    IMemoryCache cache) : IExchangeRateProvider
{
    private const string CacheKey = "ExchangeRates";
    public const string LocalCurrencyCode = "CZK";
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(options.Value.CacheDurationHours);
    private readonly CnbCzClient _cnbApiClient = cnbCzClient ?? throw new ArgumentNullException(nameof(cnbCzClient));

    private readonly ILogger<CnbExchangeRateProvider> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        if (_cache.TryGetValue(CacheKey, out IEnumerable<ExchangeRate>? cachedRates) && cachedRates != null)
            return cachedRates;

        try
        {
            // Using english as preferred language
            var response = await _cnbApiClient.DailyUsingGET_1Async(null, Lang.EN);
            _cache.Set(CacheKey, response.Rates, _cacheDuration);
            return response.Rates.Select(rate =>
                new ExchangeRate(new Currency(rate.CurrencyCode), new Currency(LocalCurrencyCode), rate.Rate ?? 0));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rates from CNB");
            return new List<ExchangeRate>();
        }
    }
}
