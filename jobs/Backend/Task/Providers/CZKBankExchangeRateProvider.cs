using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Providers
{
    public class CZKBankExchangeRateProvider
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ICacheService _cacheService;
        private const string CACHE_IDENTIFIER = "CZKBankExchangeRates";
        private const string CZECH_CODE = "CZK";


        public CZKBankExchangeRateProvider(IHttpClientService httpClientService, ICacheService cacheService)
        {
            _httpClientService = httpClientService;
            _cacheService = cacheService;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source gets the cached data if available. 
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime currentDate)
        {
            string cacheKey = FileCacheTimezoneHelper.GetCacheKeyForDate(currentDate, CACHE_IDENTIFIER);

            string cachedData = await _cacheService.GetCachedDataAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {

                return ConvertToExchangeRates(JsonConvert.DeserializeObject<ApiResponse>(cachedData), currencies, CZECH_CODE);
            }

            string url = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN&date=" + currentDate.ToString("yyyy-MM-dd");
            string responseData = await _httpClientService.FetchDataAsync(url);
            if (responseData == null) return Enumerable.Empty<ExchangeRate>();

            await _cacheService.SaveDataToCacheAsync(cacheKey, responseData);
            return ConvertToExchangeRates(JsonConvert.DeserializeObject<ApiResponse>(responseData), currencies, CZECH_CODE);
        }

        private IEnumerable<ExchangeRate> ConvertToExchangeRates(ApiResponse apiResponse, IEnumerable<Currency> currencies, string baseCurrencyCode)
        {
            return apiResponse.Rates
                    .Where(r => currencies.Any(c => c.Code == r.CurrencyCode))
                    .Select(r => new ExchangeRate(new Currency(baseCurrencyCode), new Currency(r.CurrencyCode), r.Rate));
        }
    }
}