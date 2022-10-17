using ExchangeRateUpdater.Domain.Extensions;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Domain.Caches;

/// <summary>
/// Cache service for exchange rate.
/// </summary>
public class ExchangeRateCache : IExchangeRateCache
{
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// Constructs a <see cref="ExchangeRateCache"/>
    /// </summary>
    /// <param name="memoryCache">The memory cache.</param>
    public ExchangeRateCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    /// <summary>
    /// Sets exchange rates by each date.
    /// </summary>
    /// <param name="dateTime">The date.</param>
    /// <param name="exchangeRates">The exchange rates.</param>
    public void Set(DateTime dateTime, IEnumerable<ExchangeRate>? exchangeRates)
    {
        _memoryCache.Set(dateTime.ToString("d"), exchangeRates);
    }
    
    /// <summary>
    /// Gets latest exchanges rates.
    /// </summary>
    public IEnumerable<ExchangeRate>? Get()
    {
        var currentDate = DateTime.Now.ToString("d");
        var previousWorkDate = DateTime.Now.PreviousWorkDay().ToString("d");
        
        if(_memoryCache.TryGetValue(currentDate, out IEnumerable<ExchangeRate>? exchangeRates))
        {
            return exchangeRates;
        }

        if (_memoryCache.TryGetValue(previousWorkDate, out exchangeRates))
        {
            return exchangeRates;
        }

        return exchangeRates;
    }
}