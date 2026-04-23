using Mews.ExchangeRate.Storage.Abstractions;
using Mews.ExchangeRate.Storage.Abstractions.Models;
using Mews.ExchangeRate.Storage.DistributedCache.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Mews.ExchangeRate.Storage.Abstractions.Models.StorageStatus;

namespace Mews.ExchangeRate.Storage.DistributedCache
{
    public class ExchangeRateStorageCommandRepository : IExchangeRateCommandRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public bool IsInitializedAndReady => _distributedCache.IsCacheReady();

        public ExchangeRateStorageCommandRepository(
            ILogger<ExchangeRateStorageCommandRepository> logger,
            IDistributedCache distributedCache
        )
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _distributedCache = distributedCache ?? throw new System.ArgumentNullException(nameof(distributedCache));
        }

        /// <summary>
        /// Sets the exchange rates asynchronously.
        /// </summary>
        /// <param name="exchangeRates">The exchange rates.</param>
        /// <param name="source">       The source of the data.</param>
        /// <param name="updateStatus"> The update status.</param>
        /// <returns></returns>
        public async Task<bool> SetExchangeRatesAsync(IEnumerable<Domain.Models.ExchangeRate> exchangeRates, string source, UpdateStatus? updateStatus)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await RemoveSourceAsync(source);

                var exchangeRateKeys = new HashSet<string>();

                foreach (var exchangeRate in exchangeRates)
                {
                    var key = StorageKeyBuilder.BuildExchangeRateKey(
                            exchangeRate.SourceCurrency, exchangeRate.TargetCurrency
                        );

                    _ = exchangeRateKeys.Add(key);

                    await _distributedCache.SetAsync(
                        key,
                        exchangeRate
                    );
                }

                if (string.IsNullOrWhiteSpace(source))
                {
                    return false;
                }

                await UpdateStateAsync(source, updateStatus, exchangeRateKeys);

                return true;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task RemoveSourceAsync(string source)
        {
            var currentState = _distributedCache.Get<StorageState>(StorageKeyBuilder.StorageStateKey);

            if (currentState is null)
            {
                return;
            }

            if (!currentState.TryGetValue(source, out var status))
            {
                return;
            }

            foreach (var key in status.ExchangeRateKeys)
            {
                await _distributedCache.RemoveAsync(key);
            }

            status.ExchangeRateKeys = Array.Empty<string>();
            status.LastUpdateStatus = UpdateStatus.Unknown;
            status.LastUpdateTimeStamp = DateTimeOffset.UtcNow;

            await _distributedCache.SetAsync(StorageKeyBuilder.StorageStateKey, currentState);
        }

        private async Task UpdateStateAsync(string source, UpdateStatus? updateStatus, HashSet<string> exchangeRateKeys)
        {
            var currentState = _distributedCache.Get<StorageState>(StorageKeyBuilder.StorageStateKey)
                ?? new StorageState();

            currentState.AddOrUpdate(
                source,
                new StorageStatus
                {
                    ExchangeRateKeys = exchangeRateKeys.ToArray(),
                    LastUpdateStatus = updateStatus ?? UpdateStatus.Unknown,
                    LastUpdateTimeStamp = DateTimeOffset.UtcNow,
                    Source = source
                }, (_, status) =>
                {
                    status.ExchangeRateKeys = exchangeRateKeys.ToArray();
                    status.LastUpdateStatus = updateStatus ?? UpdateStatus.Unknown;
                    status.LastUpdateTimeStamp = DateTimeOffset.UtcNow;
                    return status;
                });

            await _distributedCache.SetAsync(StorageKeyBuilder.StorageStateKey, currentState);
        }
    }
}