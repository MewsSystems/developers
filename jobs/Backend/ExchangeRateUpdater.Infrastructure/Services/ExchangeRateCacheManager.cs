using ExchangeRateUpdater.Domain.Models;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Infrastructure.Services
{
    public class ExchangeRateCacheManager(IMemoryCache memoryCache)
    {
        private readonly object _lock = new();
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Result<List<ExchangeRateRow>> GetDailyRates(string date, string url)
        {
            lock (_lock)
            {
                var rates = _memoryCache.Get<List<ExchangeRateRow>>(date);
                if (rates is not null)
                    return rates;

                return SetDailyRatesAsync(date, url).Result;
            }
        }
        private async Task<Result<List<ExchangeRateRow>>> SetDailyRatesAsync(string date, string url)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                var result = JsonConvert.DeserializeObject<ExchangeRatesResponse>(await response.Content.ReadAsStringAsync());
                if (result is null || result.Rates is null || result.Rates.Count == 0)
                    return new Result<List<ExchangeRateRow>>(new ArgumentException("No data for the requested information"));
                _memoryCache.Set(date, result.Rates, TimeOnly.MaxValue - TimeOnly.FromDateTime(DateTime.UtcNow));
                return result.Rates;
            }
            catch (Exception ex)
            {
                return new Result<List<ExchangeRateRow>>(ex);
            }
        }

    }
}
