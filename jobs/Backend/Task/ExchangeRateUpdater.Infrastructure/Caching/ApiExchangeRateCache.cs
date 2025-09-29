using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Infrastructure.Telemetry;
using PublicHoliday;
using ExchangeRateUpdater.Domain.Extensions;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Caching;

// Memory cache implementation for exchange rates that handles business day logic and provides efficient currency filtering.
public class ApiExchangeRateCache : IExchangeRateCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly CzechRepublicPublicHoliday _czechRepublicPublicHoliday = new();
    private readonly CacheSettings _cacheSettings;

    public ApiExchangeRateCache(IMemoryCache memoryCache, ILogger<ApiExchangeRateCache> logger, IOptions<CacheSettings> cacheSettings)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cacheSettings = cacheSettings?.Value ?? throw new ArgumentNullException(nameof(cacheSettings));
    }

    public Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, DateOnly date)
    {
        using var activity = ExchangeRateTelemetry.ActivitySource.StartActivity("GetCachedRates");
        activity?.SetTag("currency.count", currencies.Count());
        activity?.SetTag("date", date.ToString());
        
        if (currencies == null)
            throw new ArgumentNullException(nameof(currencies));

        var currencyList = currencies.ToList();
        if (!currencyList.Any())
            return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();

        var businessDate = GetBusinessDayForCacheCheck(date);
        var cacheKey = GetCacheKey(businessDate);
        
        try
        {
            if (_memoryCache.TryGetValue(cacheKey, out List<ExchangeRate>? cachedRates) && cachedRates != null)
            {
                var requestedCurrencyCodes = currencyList.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var filteredRates = cachedRates.Where(rate => requestedCurrencyCodes.Contains(rate.SourceCurrency.Code)).ToList();
                _logger.LogInformation($"Cache HIT - Key: {cacheKey}, Total rates: {cachedRates.Count}, Filtered rates: {filteredRates.Count}");

                if (filteredRates.Any())
                {
                    ExchangeRateTelemetry.CacheHits.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));
                    return filteredRates.AsReadOnlyList().AsMaybe().AsTask();
                }
            }
            
            _logger.LogInformation($"Cache MISS - Key: {cacheKey} not found");
            ExchangeRateTelemetry.CacheMisses.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));
            return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();
        }
        finally
        {
            ExchangeRateTelemetry.CacheOperationDuration.Record(activity?.Duration.TotalSeconds ?? 0);
        }
    }

    public Task CacheRates(IReadOnlyCollection<ExchangeRate> rates)
    {
        using var activity = ExchangeRateTelemetry.ActivitySource.StartActivity("CacheRates");
        activity?.SetTag("rates.count", rates.Count);
        
        if (rates == null)
            throw new ArgumentNullException(nameof(rates));

        if (!rates.Any())
            return Task.CompletedTask;

        try
        {
            var providerDate = rates.First().Date;

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheSettings.DefaultCacheExpiry,
                SlidingExpiration = _cacheSettings.DefaultCacheExpiry / 2,
                Size = 1
            };

            var cacheKey = GetCacheKey(providerDate);
            _memoryCache.Set(cacheKey, rates, cacheOptions);

            _logger.LogInformation($"Cache SET - Key: {cacheKey}, Rates: {rates.Count}, Provider date: {providerDate:yyyy-MM-dd}");
            ExchangeRateTelemetry.CacheOperations.Add(1, new KeyValuePair<string, object?>("rates.count", rates.Count));
            
            return Task.CompletedTask;
        }
        finally
        {
            ExchangeRateTelemetry.CacheOperationDuration.Record(activity?.Duration.TotalSeconds ?? 0);
        }
    }
    /// <summary>
    /// CNB API returns the closest business date in case we request rates for a holiday or weekend. 
    /// For today's date, if it's before 3PM, we use the previous business day since CNB publishes rates at 2:30PM.
    /// This is to match that behaviour when reading the cache.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private DateOnly GetBusinessDayForCacheCheck(DateOnly date)
    {
        var checkDate = date;
        
        var czechTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        var czechTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, czechTimeZone);
        
        if (checkDate == DateHelper.Today && czechTime.Hour < 15)
        {
            checkDate = checkDate.AddDays(-1);
        }

        while (_czechRepublicPublicHoliday.IsPublicHoliday(checkDate.ToDateTime(TimeOnly.MinValue)) || checkDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            checkDate = checkDate.AddDays(-1);
        }

        return checkDate;
    }

    private static string GetCacheKey(DateOnly businessDate)
    {
        return $"ExchangeRates_{businessDate:yyyy-MM-dd}";
    }
}
