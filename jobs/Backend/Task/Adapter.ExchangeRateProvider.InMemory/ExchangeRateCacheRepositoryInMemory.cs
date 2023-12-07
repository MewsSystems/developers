using Adapter.ExchangeRateProvider.CzechNatBank;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Serilog;
using System.Collections.Concurrent;
using System.Data;

namespace Adapter.ExchangeRateProvider.InMemory;

/// <summary>
/// In-memory cache representation.
/// </summary>
public class ExchangeRateCacheRepositoryInMemory : IExchangeRateProviderRepository
{
    private readonly IExchangeRateProviderRepository _exchangeRateProviderRepository;
    private readonly ILogger _logger;
    private ConcurrentDictionary<CacheKey, CacheValue> _fxRates;
    private readonly bool _enabled;
    private readonly TimeSpan _todayDataTtl;
    private readonly TimeSpan _otherDatesTtl;
    private readonly int _cacheSize;

    /// <summary>
    /// Constructor to build a cache in-memory.
    /// </summary>
    /// <param name="exchangeRateProviderRepository">Instance of IExchangeRateProvider.</param>
    /// <param name="logger">Instance of Serilog.ILogger.</param>
    /// <param name="cacheSize">Determines cache size.</param>
    /// <param name="enabled">Determines if the cache is enabled.</param>
    /// <param name="todayDataTtl">Timespan use for calculating expiry date for current date time.</param>
    /// <param name="otherDatesTtl">Timespan use for calculating expiry date for other dates except today.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExchangeRateCacheRepositoryInMemory(CzechNationalBankRepository exchangeRateProviderRepository, ILogger logger, int cacheSize, bool enabled, 
                                               TimeSpan todayDataTtl, TimeSpan otherDatesTtl)
    {
        _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
        _fxRates = new ConcurrentDictionary<CacheKey, CacheValue>();
        _enabled = enabled;
        _todayDataTtl = todayDataTtl;
        _otherDatesTtl = otherDatesTtl;
        _cacheSize = cacheSize;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Method that tries to get the cache value, if not present it will call the IExchangeRateProviderRepository and store the value in cache.
    /// </summary>
    /// <param name="exchangeRateDate">The date the exchange fx rates are requested for.</param>
    /// <param name="cancellationToken">CancellationToken Instance.</param>
    /// <returns>List of ExchangeRates for an earlier or the specified date.</returns>
    public async Task<IEnumerable<ExchangeRate>> GetAllFxRates(DateTime exchangeRateDate, CancellationToken cancellationToken)
    {
        var ttl = exchangeRateDate.Date < DateTime.Now.Date ? _otherDatesTtl : _todayDataTtl;
        var cacheKey = new CacheKey(exchangeRateDate, exchangeRateDate, CacheType.All, DateTime.Now, ttl);

        return await GetValueFromCacheAndStoreAsync(() => _exchangeRateProviderRepository
                                                        .GetAllFxRates(exchangeRateDate, cancellationToken), cacheKey);
    }


    /// <summary>
    /// Method that tries to get the cache value, if not present it will call the IExchangeRateProviderRepository and store the value in cache.
    /// </summary>
    /// <param name="sourceCurrency">The source currency that needs to be exchanged.</param>
    /// <param name="targetCurrency">The target currency that will be exchanged to.</param>
    /// <param name="from">The start date of the interval to search the rate from.</param>
    /// <param name="to">The end date of the interval to search the rate.</param>
    /// <param name="cancellationToken">CancellationToken Instance.</param>
    /// <returns>The list of ExchangeRates for the specified source/target currencies for an earlier or the specified date.</returns>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, 
                                                                                   DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        var ttl = from.Date < DateTime.Now.Date && to.Date < DateTime.Now.Date ? _otherDatesTtl : _todayDataTtl;
        var cacheKey = new CacheKey(from, to, CacheType.Selected, DateTime.Now, ttl);

        return await GetValueFromCacheAndStoreAsync(() => 
        _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(sourceCurrency, targetCurrency, from, to, cancellationToken), cacheKey);
    }

    /// <summary>
    /// Generalized method to either get the values from cache for the specified cache. If not present, execute the function and add the result in cache.
    /// </summary>
    /// <param name="func">Function to be executed if the specified key is not present in cache.</param>
    /// <param name="cacheKey">The cache key to search for in cache.</param>
    /// <returns>Returns the exchange rates either from cache or by calling the specific adapter.</returns>
    private async Task<IEnumerable<ExchangeRate>> GetValueFromCacheAndStoreAsync(Func<Task<IEnumerable<ExchangeRate>>> func, CacheKey cacheKey)
    {
        if (_enabled)
        {
            EvictBasedOnTtl();
            if (_fxRates.ContainsKey(cacheKey))
            {
                _logger.Information("Retrieved cache value for {Key}", cacheKey);
                return _fxRates[cacheKey].Value;
            }
        }

        var result = await func.Invoke();

        if (_enabled)
        {
            AddAndEvictCache(result, cacheKey);
        }

        return result;
    }

    /// <summary>
    /// This method adds and evicts cache values.
    /// </summary>
    /// <param name="exchangeRates">Exchange rates to be stored.</param>
    /// <param name="key">Cache key results to be stored under.</param>
    private void AddAndEvictCache(IEnumerable<ExchangeRate> exchangeRates, CacheKey key)
    {
        if (exchangeRates is null || exchangeRates.Any() == false) return;

        if (_fxRates.Count >= _cacheSize)
        {
            var toEvict = _fxRates.Count - _cacheSize;
            LRUEvict(toEvict);
        }

        var cacheInstance = new CacheValue(exchangeRates);
        var isStored = _fxRates.TryAdd(key, cacheInstance);
        if (isStored)
        {
            _logger.Information("Stored cache value for {Key}", key);
            return;
        }
        _logger.Warning("Failed to store cache value for {Key}", key);
    }

    /// <summary>
    /// LRU evict in-memory logic. I used LRU because of a few assumptions:
    /// - More requests will be based on recent dates than on earlier dates.
    /// </summary>
    /// <param name="toEvict">the number of instances need to be evicted.</param>
    private void LRUEvict(int toEvict)
    {
        var validKeyValues = _fxRates.OrderBy(keyValue => keyValue.Key.LastAccessedTime).Take(_cacheSize - toEvict);

        _fxRates = new ConcurrentDictionary<CacheKey, CacheValue>(validKeyValues);
    }

    /// <summary>
    /// Evict cache instances that are already expired.
    /// </summary>
    private void EvictBasedOnTtl()
    {
        var validKeyValues = _fxRates.Where(keyValue => keyValue.Key.ExpiryDate > DateTime.Now);

        _fxRates = new ConcurrentDictionary<CacheKey, CacheValue>(validKeyValues);
    }
}
