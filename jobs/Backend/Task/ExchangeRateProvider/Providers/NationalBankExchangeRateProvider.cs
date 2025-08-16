using ExchangeRateProvider.Contract.Models;
using ExchangeRateProvider.Exceptions;
using ExchangeRateProvider.Models.NationalBank;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System;

namespace ExchangeRateProvider.Providers
{
    public class NationalBankExchangeRateProvider : IExchangeRateProvider
    {
        public string API_EXCHANGE_RATES_URL { get => $"{_apiBaseUrl}/exrates/daily?lang=EN"; }

        private static readonly string CACHE_KEY = "ExchangeRates";
        private static readonly int CACHE_EXPIRY_SECONDS = 300;

        private readonly string _apiBaseUrl;

        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly ILogger<IExchangeRateProvider> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public NationalBankExchangeRateProvider(IMemoryCache cache, IConfiguration configuration, ILogger<IExchangeRateProvider> logger, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _config = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = _config.GetSection("PublicApi:Url").Value ?? throw new Exception("PublicApi:Url not defined in config");
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            IEnumerable<ExchangeRate> allExchangeRates = await GetAllExchangeRatesAsync();
            return allExchangeRates.Where(e => currencies.Contains(e.TargetCurrency));
        }
        private async Task<IEnumerable<ExchangeRate>> GetAllExchangeRatesAsync()
        {
            IEnumerable<ExchangeRate> rates = _cache.Get<IEnumerable<ExchangeRate>>(CACHE_KEY);
            if (rates != null) return rates;

            rates = await GetFreshExchangeRatesAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(CACHE_EXPIRY_SECONDS));

            _cache.Set(CACHE_KEY, rates, cacheEntryOptions);

            return rates;
        }

        private async Task<IEnumerable<ExchangeRate>> GetFreshExchangeRatesAsync()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                using HttpResponseMessage response = await httpClient.GetAsync(API_EXCHANGE_RATES_URL);
                string apiResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error status while contacting national bank exchange rate api. Status: {response.StatusCode}, Msg: {response.Content}");
                    throw new BaseApiUnavailableException();
                }

                NationalBankExchangeRateResponse res = JsonConvert.DeserializeObject<NationalBankExchangeRateResponse>(apiResponse);
                if (res?.Rates == null || res.Rates.Count() == 0)
                {
                    _logger.LogError($"National bank exchange rate api response empty. Status: {response.StatusCode}, Msg: {response.Content}");
                    return new List<ExchangeRate>();
                }

                return res.Rates.Select(r => r.ToExchangeRate());
            }
        }
    }
}
