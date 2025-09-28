using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Domain.Models;
using PublicHoliday;
using ExchangeRateUpdater.Domain.Extensions;

namespace ExchangeRateUpdater.Infrastructure.Caching;

// Memory cache implementation for exchange rates that handles business day logic and provides efficient currency filtering.
public class ApiExchangeRateCache : IExchangeRateCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ApiExchangeRateCache> _logger;
    private readonly CzechRepublicPublicHoliday _czechRepublicPublicHoliday = new();

    public ApiExchangeRateCache(IMemoryCache memoryCache, ILogger<ApiExchangeRateCache> logger)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<Maybe<IReadOnlyList<ExchangeRate>>> GetCachedRates(IEnumerable<Currency> currencies, Maybe<DateTime> date)
    {
        if (currencies == null)
            throw new ArgumentNullException(nameof(currencies));

        var currencyList = currencies.ToList();
        if (!currencyList.Any())
            return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();

        var targetDate = date.TryGetValue(out var dateValue) ? dateValue.Date : DateTime.Today;
        var businessDate = GetBusinessDayForCacheCheck(targetDate);
        var cacheKey = GetCacheKey(businessDate);
        
        if (_memoryCache.TryGetValue(cacheKey, out List<ExchangeRate> cachedRates))
        {
            var requestedCurrencyCodes = currencyList.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var filteredRates = cachedRates.Where(rate => requestedCurrencyCodes.Contains(rate.SourceCurrency.Code)).ToList();
            _logger.LogInformation($"Cache HIT - Key: {cacheKey}, Total rates: {cachedRates.Count}, Filtered rates: {filteredRates.Count}");

            if (filteredRates.Any())
            {
                return filteredRates.AsReadOnlyList().AsMaybe().AsTask();
            }
        }
        
        _logger.LogInformation($"Cache MISS - Key: {cacheKey} not found");
        return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();
    }

    public Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry)
    {
        if (rates == null)
            throw new ArgumentNullException(nameof(rates));

        if (!rates.Any())
            return Task.CompletedTask;

        // Get the provider date and its corresponding business day
        var providerDate = rates.First().Date.Date;
        var businessDate = GetBusinessDayForCacheCheck(providerDate); // Should return the same date

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheExpiry,
            SlidingExpiration = cacheExpiry / 2,
            Size = 1
        };

        var cacheKey = GetCacheKey(businessDate);
        _memoryCache.Set(cacheKey, rates, cacheOptions);

        _logger.LogInformation($"Cache SET - Key: {cacheKey}, Rates: {rates.Count}, Business date: {businessDate:yyyy-MM-dd}, Provider date: {providerDate:yyyy-MM-dd}");
        return Task.CompletedTask;
    }
    
    private DateTime GetBusinessDayForCacheCheck(DateTime date)
    {
        var checkDate = date.Date;
        
        while (_czechRepublicPublicHoliday.IsPublicHoliday(checkDate) || checkDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            checkDate = checkDate.AddDays(-1);
        }

        return checkDate;
    }

    private static string GetCacheKey(DateTime businessDate)
    {
        return $"ExchangeRates_{businessDate:yyyy-MM-dd}";
    }
}
