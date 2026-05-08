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

        try
        {
            // First, try to get cached rates for the exact requested date
            var exactDateKey = GetCacheKey(date);
            if (TryGetCachedRates(exactDateKey, currencyList, out var cachedRatesT))
                return cachedRatesT.AsTask();

            // If exact date not found, check if it is a business day; if not, find the previous business day
            var businessDate = GetBusinessDayForCacheCheck(date);
            if (businessDate != date){
                var businessDateKey = GetCacheKey(businessDate);
                if (TryGetCachedRates(businessDateKey, currencyList, out var cachedRates))
                    return cachedRates.AsTask();
            }
            
            _logger.LogInformation($"Cache MISS - No rates found for date {date:yyyy-MM-dd} or previous business day {businessDate:yyyy-MM-dd}");
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
    
    private bool TryGetCachedRates(string cacheKey, List<Currency> currencyList, out Maybe<IReadOnlyList<ExchangeRate>> cachedRatesValue)
    {
        if (_memoryCache.TryGetValue(cacheKey, out List<ExchangeRate>? cachedRates) && cachedRates != null)
        {
            _logger.LogInformation($"Cache HIT - date key: {cacheKey}");
            ExchangeRateTelemetry.CacheHits.Add(1, new KeyValuePair<string, object?>("currency.count", currencyList.Count));

            var requestedCurrencyCodes = currencyList.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var filteredRates = cachedRates.Where(rate => requestedCurrencyCodes.Contains(rate.SourceCurrency.Code)).ToList();

            if (filteredRates.Any())
            {
                cachedRatesValue = filteredRates.AsReadOnlyList().AsMaybe();
                return true;
            }
        }
        
        cachedRatesValue = Maybe<IReadOnlyList<ExchangeRate>>.Nothing;
        return false;
    }

    /// <summary>
    /// CNB API returns the closest business date in case we request rates for a holiday or weekend or simply before 2:30 PM when the rates are published.
    /// This is to match that behaviour when reading the cache.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private DateOnly GetBusinessDayForCacheCheck(DateOnly date)
    {
        var checkDate = date;
        
        if (checkDate == DateHelper.Today)
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
