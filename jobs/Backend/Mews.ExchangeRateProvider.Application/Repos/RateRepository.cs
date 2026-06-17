using Mews.ExchangeRateProvider.Application.Abstractions;
using Mews.ExchangeRateProvider.Application.Utils;
using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;
using Mews.ExchangeRateProvider.Infrastructure.Abstractions;
using Mews.ExchangeRateProvider.Infrastructure.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace Mews.ExchangeRateProvider.Application.Repos
{
    public class RateRepository : IRateRepository
    {
        private readonly ICNBCacheProvider _cacheProvider;
        private readonly ILogger<RateRepository> _logger;
        private readonly ICNBClient _cnbClient;
        private const string _defaultCNBCurrency = "CZK";
        public RateRepository(ICNBCacheProvider cacheProvider,
           ILogger<RateRepository> logger,
           ICNBClient cNBClient)
        {
            _cacheProvider = cacheProvider;
            _logger = logger;
            _cnbClient = cNBClient;
        }
        /// <summary>
        /// check for cached items, return from cache if it is populated, otherwise call CNB APIand and create ExchangeRate from result
        /// </summary>
        /// <param name="date"></param>
        /// <param name="lang"></param>
        /// <param name="getAllRates"></param>
        /// <returns>IEnumerable<ExchangeRate></returns>
        public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(string date, string lang, bool getAllRates)
        {
            var currentDate = DateTime.Now.Date;
            var cacheKey = getAllRates ? $"{CacheKeys.CNBDailyAllRates}{date}{lang}{getAllRates}" : $"{CacheKeys.CNBDaily}{date}{lang}{getAllRates}";
            var cacheOptions = GetCacheOptions();

            var cachedRates = GetRatesFromCache(cacheKey);
            if (cachedRates.Any())
            {
                return cachedRates;
            }
            var cnbDailyRates = await _cnbClient.GetDailyRatesCNBAsync(date, lang);
            if (cnbDailyRates is not null)
            {
                IEnumerable<ExchangeRate> mappedRates;

                if (getAllRates)
                {
                    mappedRates = cnbDailyRates
                        .Where(dailyRate => dailyRate != null && dailyRate.CurrencyCode != null)
                        .Select(dailyRate => new ExchangeRate(
                            new Currency(dailyRate.CurrencyCode!),
                            new Currency(_defaultCNBCurrency),
                            dailyRate.Amount != 0 ? decimal.Divide(dailyRate.Rate, dailyRate.Amount) : 0));
                }
                else
                {
                    var currencies = ValidCurrenciesList.currencies.Select(c => c.Code).ToHashSet();
                    mappedRates = cnbDailyRates
                        .Where(rate => rate != null && rate.CurrencyCode != null && currencies.Contains(rate.CurrencyCode))
                        .Select(exchangeRate => new ExchangeRate(
                            new Currency(exchangeRate.CurrencyCode!),
                            new Currency(_defaultCNBCurrency),
                            exchangeRate.Amount != 0 ? decimal.Divide(exchangeRate.Rate, exchangeRate.Amount) : 0));
                }
                _cacheProvider.SetCache(cacheKey, mappedRates, cacheOptions);
                return mappedRates;
            }
            return Enumerable.Empty<ExchangeRate>();
        }
        private MemoryCacheEntryOptions GetCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(4))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        }
        private IEnumerable<ExchangeRate> GetRatesFromCache(string cacheKey)
        {
            var returnedCache = _cacheProvider.GetFromCache<IEnumerable<ExchangeRate>>(cacheKey);
            if (returnedCache is not null && returnedCache.Any())
            {
                return returnedCache;
            }
            return Enumerable.Empty<ExchangeRate>();
        }
    }
}
