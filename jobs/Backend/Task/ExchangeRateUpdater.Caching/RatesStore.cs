using ExchangeRateUpdater.Provider.Cnb.Dtos;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Caching
{
    public class RatesStore : IRatesStore
    {
        private const string CacheKey = "CNB:LatestResponse";
        private readonly IMemoryCache _cache;
        private readonly ILogger<RatesStore> _logger;
        private static readonly MemoryCacheEntryOptions EntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365)
        };

        public RatesStore(IMemoryCache cache, ILogger<RatesStore> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public CnbResponse? Get()
        {
            return _cache.Get<CnbResponse>(CacheKey);
        }

        public void SetIfNewer(CnbResponse candidate)
        {
            if (candidate?.Rates == null || candidate.Rates.Count == 0)
                return;

            var incomingValidForDate = candidate.GetValidForDate();
            var current = Get();
            var currentValidForDate = current.GetValidForDate();

            if (current == null 
                || (incomingValidForDate != null && incomingValidForDate >= currentValidForDate))
            {
                _cache.Set(CacheKey, candidate, EntryOptions);
                _logger.LogInformation($"RatesStore: Stored new rates with validFor {incomingValidForDate}");
            }
        }
    }
}
