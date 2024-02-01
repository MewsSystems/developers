using Mews.ExchangeRate.Domain.Models;
using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Storage.DistributedCache.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mews.ExchangeRate.Storage.DistributedCache
{
    public class ExchangeRateStorageQueryRepository : IExchangeRateQueryRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;
        public bool IsInitializedAndReady => _distributedCache.IsCacheReady();

        public ExchangeRateStorageQueryRepository(
            ILogger<ExchangeRateStorageQueryRepository> logger,
            IDistributedCache distributedCache
        )
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _distributedCache = distributedCache ?? throw new System.ArgumentNullException(nameof(distributedCache));
        }

        /// <summary>
        /// Gets the exchange rate for the given currency asynchronously.
        /// </summary>
        /// <param name="sourceCurrency">The origin currency.</param>
        /// <returns></returns>
        public async Task<Domain.Models.ExchangeRate> GetExchangeRateAsync(Currency sourceCurrency)
        {
            var value = await _distributedCache.GetAsync<Domain.Models.ExchangeRate>(
                StorageKeyBuilder.BuildExchangeRateKey(sourceCurrency, Currency.Default)
            );

            return value;
        }

        /// <summary>
        /// Gets the exchange rates asynchronously.
        /// </summary>
        /// <param name="sourceCurrencies"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Domain.Models.ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> sourceCurrencies)
        {
            var exchangeRatesTasks = sourceCurrencies
                .Select(
                currency => _distributedCache.GetAsync<Domain.Models.ExchangeRate>(
                    StorageKeyBuilder.BuildExchangeRateKey(currency, Currency.Default)
                    )
                );

            var exchangeRates = await Task.WhenAll(exchangeRatesTasks);
            return exchangeRates;
        }
    }
}