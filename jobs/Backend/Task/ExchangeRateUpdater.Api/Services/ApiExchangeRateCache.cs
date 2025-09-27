using Microsoft.Extensions.Caching.Memory;
using ExchangeRateUpdater.Core.Common;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using PublicHoliday;
using ExchangeRateUpdater.Core.Extensions;

namespace ExchangeRateUpdater.Api.Services;

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

        var targetDate = GetBusinessDayForCacheCheck(date);

        if (_memoryCache.TryGetValue(targetDate, out var cachedRates) && cachedRates is IEnumerable<ExchangeRate> allRates)
        {
            var requestedCurrencyCodes = currencyList.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var filteredRates = allRates.Where(rate => requestedCurrencyCodes.Contains(rate.SourceCurrency.Code)).ToList();
            _logger.LogInformation($"Cache hit for date {targetDate:yyyy-MM-dd}, returning {filteredRates.Count} rates");
            
            if (filteredRates.Any())
            {
                return filteredRates.AsReadOnlyList().AsMaybe().AsTask();
            }
        }

        _logger.LogInformation($"Cache miss for date {targetDate:yyyy-MM-dd} and currencies: {string.Join(", ", currencyList.Select(c => c.Code))}");
        return Maybe<IReadOnlyList<ExchangeRate>>.Nothing.AsTask();
    }

    public Task CacheRates(IReadOnlyCollection<ExchangeRate> rates, TimeSpan cacheExpiry)
    {
        if (rates == null)
            throw new ArgumentNullException(nameof(rates));

        if (!rates.Any())
            return Task.CompletedTask;

        // Cache all rates with the date returned by the CNB provider
        var date = rates.First().Date.Date;

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheExpiry,
            SlidingExpiration = cacheExpiry / 2,
            Size = rates.Count
        };

        _memoryCache.Set(date, rates, cacheOptions);

        _logger.LogInformation($"Cached {rates.Count} exchange rates for date {date:yyyy-MM-dd}, expires in {cacheExpiry.TotalMinutes} minutes");

        return Task.CompletedTask;
    }
    
    private static DateTime GetBusinessDayForCacheCheck(Maybe<DateTime> date)
    {
        if (!date.TryGetValue(out var dateValue))
            dateValue = DateTime.Today;

        while (new CzechRepublicPublicHoliday().IsPublicHoliday(dateValue) || dateValue.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            dateValue = dateValue.AddDays(-1);
        }

        return dateValue;
    }
}
