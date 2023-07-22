using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.Models;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.ExtensionMethods;
using MassTransit;
using MassTransit.Futures.Contracts;

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
                var exchangeRateDate = DateTime.UtcNow.ToCzechNationalBankExchangeNow();

                if (_cache.TryGetValue(exchangeRateDate, out Dictionary<string, ExchangeRate> cachedRates))
                {
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(cachedRates);
                }

                var cnbClient = _httpClientFactory.CreateClient("CzechNationalBankApi");
                var centralBankRatesResult = await GetCentralBankRates(cnbClient,exchangeRateDate);
                var otherCurrencyRatesResult = await GetOtherCurrencyRates(cnbClient);

                if(!centralBankRatesResult.IsSuccess && !otherCurrencyRatesResult.IsSuccess)
                {
                    _logger.Error("There is a problem retrieving rates, daily FX result {@centralBankRates}, other FX {@otherCurrencyRates}", centralBankRatesResult, otherCurrencyRatesResult);
                    return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(exchangeRates, $"We are having issues retrieving exchange rates");
                }

                foreach (var centralBankRate in centralBankRatesResult.Content)
                {
                    exchangeRates.Add(centralBankRate.CurrencyCode, new ExchangeRate(new Currency(centralBankRate.CurrencyCode),_targetCurrency,centralBankRate.Rate));
                }         
                foreach (var otherCurrencyRate in otherCurrencyRatesResult.Content)
                {
                    exchangeRates.Add(otherCurrencyRate.CurrencyCode, new ExchangeRate(new Currency(otherCurrencyRate.CurrencyCode),_targetCurrency, otherCurrencyRate.Rate));
                }
                // Cache for 10 minutes
                _cache.Set(exchangeRateDate, exchangeRates, TimeSpan.FromMinutes(10));
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Success(exchangeRates);

            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error while retrieving exchanges");
                return NonNullResponse<Dictionary<string, ExchangeRate>>.Fail(exchangeRates, exception.Message);
            }
        }

        private async Task<NonNullResponse<List<RateDto>>>GetCentralBankRates(HttpClient client, string fxDate)
        {
            return await GeRates(client, $"exrates/daily?date={fxDate}&lang=EN");
        }        
        
        private async Task<NonNullResponse<List<RateDto>>>GetOtherCurrencyRates(HttpClient client)
        {
            var exchangeRatesDate = DateTime.UtcNow.ToOtherCurrenciesExchangeNow();
            return await GeRates(client, $"fxrates/daily-month?lang=EN&yearMonth={exchangeRatesDate}");
        }

        private async Task<NonNullResponse<List<RateDto>>> GeRates(HttpClient client, string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error("The api has responded with {code} while retrieving rates: {@response}",
                        response.StatusCode, response);
                    return NonNullResponse<List<RateDto>>.Fail(new List<RateDto>(), $"Api responded with code {response.StatusCode}");
                }

                var deserializedResponse = JsonSerializer.Deserialize<CnbApiRatesResponse>(await response.Content.ReadAsStringAsync());
                if (deserializedResponse != null)
                {
                    return NonNullResponse<List<RateDto>>.Success(deserializedResponse.Rates);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Error while retrieving central bank rates");
            }

            return NonNullResponse<List<RateDto>>.Fail(new List<RateDto>(), "Could not retrieve Central Bank Rates");
        }

    }
}
