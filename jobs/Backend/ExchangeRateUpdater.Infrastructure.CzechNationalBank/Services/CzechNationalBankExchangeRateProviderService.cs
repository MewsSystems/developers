using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank.Services
{
    public class CzechNationalBankExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        private readonly Currency _targetCurrency = new("CZK");
        private readonly IMemoryCache _cache;


        public CzechNationalBankExchangeRateProviderService(IHttpClientFactory httpClientFactory, ILogger logger, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<NonNullResponse<Dictionary<string, ExchangeRate>>> GetExchangeRates()
        {
            var exchangeRates = new Dictionary<string, ExchangeRate>();
            try
            {
                var cnbClient = _httpClientFactory.CreateClient("CzechNationalBankApi");
                var exchangeRatesDate = DateTime.Now.ToString("yyyy-MM-dd");

                if (_cache.TryGetValue(exchangeRatesDate, out Dictionary<string, ExchangeRate> cachedRates))
                {
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(cachedRates);
                }

                var response = await cnbClient.GetAsync($"exrates/daily?date={exchangeRatesDate}&lang=EN");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error("The api has responded with {code}: {@response}",response.StatusCode,response);
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(exchangeRates,$"Api responded with code {response.StatusCode}");
                }
                var deserializedResponse = JsonSerializer.Deserialize<DailyRatesResponse>(await response.Content.ReadAsStringAsync());
                if (deserializedResponse != null)
                {
                    exchangeRates = deserializedResponse.Rates.ToDictionary(rate => rate.CurrencyCode, rate => new ExchangeRate(new Currency(rate.CurrencyCode),_targetCurrency, rate.Rate));
                    // Cache for 10 minutes
                    _cache.Set(exchangeRatesDate, exchangeRates, TimeSpan.FromMinutes(10)); 
                }
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(exchangeRates);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error while retrieving exchanges");
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(exchangeRates, exception.Message);
            }
        }
    }
}
