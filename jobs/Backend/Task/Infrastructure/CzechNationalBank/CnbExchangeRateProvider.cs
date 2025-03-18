using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CnbExchangeRateProvider(HttpClient httpClient, IMemoryCache cache, TimeSpan cacheDuration)
        {
            _cache = cache;
            _cacheDuration = cacheDuration;
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        /// <summary>
        /// Provides exchange rates from the Czech National Bank.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> rates;

            var todayDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");

            string cacheKey = $"ExchangeRates_{todayDate:yyyyMMdd}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> cachedRates))
            {
                rates = cachedRates;
            }

            else
            {
                var CnbExchangeRateResponse = await FetchExchangeRatesAsync();

                rates = CnbExchangeRateMapper.Map(CnbExchangeRateResponse);

                if (rates.Any() && rates != null)
                {
                    DateTime validFor = DateTime.Parse(CnbExchangeRateResponse.Rates.First().ValidFor).Date;

                    string newCacheKey = $"ExchangeRates_{validFor:yyyyMMdd}";

                    _cache.Set(newCacheKey, rates, _cacheDuration);
                }
            }

            var requestedCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            return rates.Where(rate => requestedCodes.Contains(rate.TargetCurrency.Code));
        }

        private async Task<CnbExchangeRateResponse> FetchExchangeRatesAsync()
        {
            string apiUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN"; //TODO: move to some config file

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request to {apiUrl} failed with status code {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CnbExchangeRateResponse>(responseContent, _jsonSerializerOptions);
        }
    }
}

