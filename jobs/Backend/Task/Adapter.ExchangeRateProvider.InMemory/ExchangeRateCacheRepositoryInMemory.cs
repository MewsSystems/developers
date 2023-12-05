using Adapter.ExchangeRateProvider.CzechNatBank;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Serilog;
using System.Collections.Concurrent;
using System.Data;

namespace Adapter.ExchangeRateProvider.InMemory
{
    public class ExchangeRateCacheRepositoryInMemory : IExchangeRateProviderRepository
    {
        private readonly IExchangeRateProviderRepository _exchangeRateProviderRepository;
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, CacheInstance> _fxRates;
        private readonly bool _enabled;
        private readonly int _cacheSize;


        public ExchangeRateCacheRepositoryInMemory(CzechNationalBankRepository exchangeRateProviderRepository, ILogger logger, int cacheSize, bool enabled)
        {
            _exchangeRateProviderRepository = exchangeRateProviderRepository ?? throw new ArgumentNullException(nameof(exchangeRateProviderRepository));
            _fxRates = new ConcurrentDictionary<string, CacheInstance>();
            _enabled = enabled;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExchangeRate>> GetAllFxRates(DateTime exchangeRateDate)
        {
            var cacheKey = $"All.{exchangeRateDate}";

            return await GetValueFromCacheAndStoreAsync(() => _exchangeRateProviderRepository.GetAllFxRates(exchangeRateDate), cacheKey);
        }

       

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency, DateTime from, DateTime to)
        {
            var cacheKey = $"Selected.{from}.{to}";

            return await GetValueFromCacheAndStoreAsync(() => _exchangeRateProviderRepository.GetExchangeRateForCurrenciesAsync(sourceCurrency, targetCurrency, from, to), cacheKey);
        }

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


        private void AddAndEvictCache(IEnumerable<ExchangeRate> exchangeRates, string key)
        {
            if (_fxRates.Count >= _cacheSize)
            {
                var toEvict = _fxRates.Count - _cacheSize;
                LRUEvict(exchangeRates, toEvict);
            }

            var cacheInstance = new CacheInstance(exchangeRates);
            _fxRates.TryAdd(key, cacheInstance);
        }

        private void LRUEvict(IEnumerable<ExchangeRate> exchangeRates, int toEvict)
        {
            var orderedValues = _fxRates.OrderBy(keyValuePair => keyValuePair.Value.AccessedTime).Take(toEvict);

            foreach (var keyValue in orderedValues)
            {
                _fxRates.Remove(keyValue.Key, out var _);
            }
        }
    }
}
