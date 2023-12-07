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
    private readonly ConcurrentDictionary<string, CacheInstance> _fxRates;
    private readonly bool _enabled;
    private readonly int _cacheSize;

    /// <summary>
    /// Constructor to build a cache in-memory.
    /// </summary>
    /// <param name="exchangeRateProviderRepository">Instance of IExchangeRateProvider.</param>
    /// <param name="logger">Instance of Serilog.ILogger.</param>
    /// <param name="cacheSize">Determines cache size.</param>
    /// <param name="enabled">Determines if the cache is enabled.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExchangeRateCacheRepositoryInMemory(CzechNationalBankRepository exchangeRateProviderRepository, ILogger logger, int cacheSize, bool enabled)
    {
        _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
        _fxRates = new ConcurrentDictionary<string, CacheInstance>();
        _enabled = enabled;
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
        var cacheKey = $"All.{exchangeRateDate}";

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
        var cacheKey = $"Selected.{from}.{to}";

        return await GetValueFromCacheAndStoreAsync(() => 
        _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(sourceCurrency, targetCurrency, from, to, cancellationToken), cacheKey);
    }

    /// <summary>
    /// Generalized method to either get the values from cache for the specified cache. If not present, execute the function and add the result in cache.
    /// </summary>
    /// <param name="func">Function to be executed if the specified key is not present in cache.</param>
    /// <param name="cacheKey">The cache key to search for in cache.</param>
    /// <returns>Returns the exchange rates either from cache or by calling the specific adapter.</returns>
    private async Task<IEnumerable<ExchangeRate>> GetValueFromCacheAndStoreAsync(Func<Task<IEnumerable<ExchangeRate>>> func, string cacheKey)
    {
        if (_enabled)
        {
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
    private void AddAndEvictCache(IEnumerable<ExchangeRate> exchangeRates, string key)
    {
        if (_fxRates.Count >= _cacheSize)
        {
            var toEvict = _fxRates.Count - _cacheSize;
            LRUEvict(toEvict);
        }

        var cacheInstance = new CacheInstance(exchangeRates);
        _fxRates.TryAdd(key, cacheInstance);
    }

    /// <summary>
    /// LRU evict in-memory logic. I used LRU because of a few assumptions:
    /// - More requests will be based on recent dates than on earlier dates.
    /// </summary>
    /// <param name="toEvict">the number of instances need to be evicted.</param>
    private void LRUEvict(int toEvict)
    {
        var orderedValues = _fxRates.OrderBy(keyValuePair => keyValuePair.Value.AccessedTime).Take(toEvict);

        foreach (var keyValue in orderedValues)
        {
            _fxRates.Remove(keyValue.Key, out var _);
        }
    }
}
