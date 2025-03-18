using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CnbExchangeRateProvider> _logger;

        public CnbExchangeRateProvider(
            HttpClient httpClient,
            IMemoryCache cache,
            TimeSpan cacheDuration,
            ILogger<CnbExchangeRateProvider> logger)
        {
            _cache = cache;
            _cacheDuration = cacheDuration;
            _httpClient = httpClient;
            _logger = logger;

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

            _logger.LogInformation("Fetching exchange rates for {Count} currencies at {Time}",
                currencies?.Count() ?? 0, todayDate);

            if (_cache.TryGetValue(cacheKey, out IEnumerable<ExchangeRate> cachedRates))
            {
                _logger.LogInformation("Cache hit for key {CacheKey}", cacheKey);
                rates = cachedRates;
            }

            else
            {
                _logger.LogInformation("Cache miss for key {CacheKey}. Calling CNB API...", cacheKey);

                try
                {
                    var CnbExchangeRateResponse = await FetchExchangeRatesAsync();

                    rates = CnbExchangeRateMapper.Map(CnbExchangeRateResponse);

                    if (rates.Any() && rates != null)
                    {
                        DateTime validFor = DateTime.Parse(CnbExchangeRateResponse.Rates.First().ValidFor).Date;

                        string newCacheKey = $"ExchangeRates_{validFor:yyyyMMdd}";

                        _logger.LogInformation("Caching exchange rates under key {CacheKey} for {Duration} hours",
                            newCacheKey, _cacheDuration.TotalHours);

                        _cache.Set(newCacheKey, rates, _cacheDuration);

                        _logger.LogInformation("Caching successful");
                    }
                    else
                    {
                        _logger.LogWarning("CNB API returned no exchange rates.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching or mapping exchange rates from CNB API.");
                    throw;
                }                
            }

            var requestedCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);

            var filteredRates = rates.Where(rate => requestedCodes.Contains(rate.TargetCurrency.Code));

            _logger.LogInformation("Returning {Count} filtered exchange rates", filteredRates.Count());

            return filteredRates;
        }

        private async Task<CnbExchangeRateResponse> FetchExchangeRatesAsync()
        {
            string apiUrl = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN"; //TODO: move to some config file

            _logger.LogDebug("Making request to CNB API at {Url}", apiUrl);

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("CNB API request to {Url} failed with status code {StatusCode}", apiUrl, response.StatusCode);
                throw new HttpRequestException($"Request to {apiUrl} failed with status code {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CnbExchangeRateResponse>(responseContent, _jsonSerializerOptions);
        }
    }
}

