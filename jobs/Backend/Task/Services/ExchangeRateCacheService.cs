using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateCacheService(IMemoryCache cache) : IExchangeRateCacheService
    {
        private const string InvalidCodesKey = "InvalidCodes";

        public ICollection<ExchangeRate> GetCachedRates(IEnumerable<string> currencyCodes)
        {
            var results = new List<ExchangeRate>();
            foreach (var code in currencyCodes)
            {
                var key = code.ToUpperInvariant();
                if (cache.TryGetValue(key, out ExchangeRate? rate) && rate is not null)
                    results.Add(rate);
            }
            return results;
        }

        public void SetRates(IEnumerable<ExchangeRate> rates)
        {
            foreach (var rate in rates)
            {
                var key = rate.SourceCurrency.Code.ToUpperInvariant();
                cache.Set(key, rate);
            }
        }

        public void UpdateInvalidCodes(IEnumerable<string> codes)
        {
            var existing = GetInvalidCodes();
            foreach (var code in codes)
                existing.Add(code);

            cache.Set(InvalidCodesKey, existing);
        }

        public HashSet<string> GetInvalidCodes() =>
        cache.TryGetValue<HashSet<string>>(InvalidCodesKey, out var codes)
        ? codes
        : [];
    }
}
