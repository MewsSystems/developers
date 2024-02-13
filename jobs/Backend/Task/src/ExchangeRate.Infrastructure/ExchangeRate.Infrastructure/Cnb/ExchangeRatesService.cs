using ExchangeRate.Application.Services;
using ExchangeRate.Infrastructure.Cnb.Mappers;
using ExchangeRate.Infrastructure.Cnb.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRate.Infrastructure.Cnb;

public class ExchangeRatesService(IMemoryCache cache, IExchangeRateFetcher fetcher) : IExchangeRatesService
{
    //Note: after implementing this cache mechanism, I realized that some kind of CRON scheduler would be better for this task.
    private readonly IMemoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly IExchangeRateFetcher _fetcher = fetcher ?? throw new ArgumentNullException(nameof(fetcher));
 
    private const string CzechTimeZoneId = "Central Europe Standard Time";
    
    /// <summary>CNB updates its daily exchange rates once a day at 14:30.</summary>
    private static readonly TimeSpan ExchangeRatesUpdateTime = new(14, 30, 0);
    
    public async Task<IEnumerable<Domain.ExchangeRate>> GetCurrentExchangeRates()
    {
        var czechDateTime = GetCzechDateTime();
        var date = czechDateTime.Date.ToString("yyyy-MM-dd");
        const string cacheKey = "ExchangeRates";
        
        if (!_cache.TryGetValue(cacheKey, out CachedValue? cachedValue) //when cache not found
            || cachedValue!.ValidFor.Date < ExpectedLastValidCacheDateTime(czechDateTime).Date) //when cache is outdated
        {
            var exchangeRates = (await _fetcher.GetDailyExchangeRates(date)).ToList();

            var validFor = DateTime.Parse(exchangeRates.First().ValidFor);
            _cache.Set(cacheKey, new CachedValue(validFor, exchangeRates), TimeSpan.FromHours(24)); //the 24h absolute expiration is a fail safe mechanism

            return exchangeRates.ToDomain();
        }
        
        return cachedValue.ExchangeRates.ToDomain();
    }

    //Note: this method does not account for holidays. The production ready version should account for it.
    private DateTime ExpectedLastValidCacheDateTime(DateTime currentCzechDateTime)
    {
        if (currentCzechDateTime.TimeOfDay >= ExchangeRatesUpdateTime && IsWorkDay(currentCzechDateTime) )
        {
            return currentCzechDateTime;
        }
        
        if (currentCzechDateTime.TimeOfDay < ExchangeRatesUpdateTime && IsWorkDay(currentCzechDateTime) && currentCzechDateTime.DayOfWeek is not DayOfWeek.Monday)
        {
            return currentCzechDateTime.AddDays(-1);
        }

        if (currentCzechDateTime.DayOfWeek is DayOfWeek.Saturday)
        {
            return currentCzechDateTime.AddDays(-1);
        }
        
        if (currentCzechDateTime.DayOfWeek is DayOfWeek.Sunday)
        {
            return currentCzechDateTime.AddDays(-2);
        }

        throw new Exception($"The method {nameof(ExpectedLastValidCacheDateTime)} should not reach this point.");
    }

    private bool IsWorkDay(DateTime currentCzechDateTime) => 
        currentCzechDateTime.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday or DayOfWeek.Thursday or DayOfWeek.Friday;

    //Note: time provides should be injected in order to make it more testable
    private static DateTime GetCzechDateTime()
    {
        var czechTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CzechTimeZoneId);
        var utcNow = DateTime.UtcNow;
        var czechDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, czechTimeZone);

        return czechDateTime;
    }
}