using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Exchanges.Mappers;
using System;
using System.Net.Http;
using ExchangeRateUpdater.Model.Cnb;

namespace ExchangeRateUpdater.Exchanges.Providers
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string _baseUrl;
        private readonly IConfiguration _config;
        private readonly IHttpResilientClient _httpResilientClient;
        private readonly ILogger _logger;

        public CnbExchangeRateProvider(IConfiguration config, IHttpResilientClient httpResilientClient, ILogger logger) 
        {
            _config = config;
            _httpResilientClient = httpResilientClient;
            _logger = logger;

            if (_config["EXCHANGES:CNB_BASE_URL"] == null)
            {
                var errNotSet = "EXCHANGES:CNB_BASE_URL variable is not set";
                _logger.LogCritical(errNotSet);
                throw new InvalidOperationException(errNotSet);
            }
            if (_config["EXCHANGES:CNB_TARGET_CURRENCY"] == null)
            {
                var errCurrencyNotSet = "EXCHANGES:CNB_TARGET_CURRENCY variable is not set";
                _logger.LogCritical(errCurrencyNotSet);
                throw new InvalidOperationException(errCurrencyNotSet);
            }

            _baseUrl = config["EXCHANGES:CNB_BASE_URL"];
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string dailyExRateUrl = _config["EXCHANGES:CNB_DAILY_EXRATE_URL"];
            if (_config["EXCHANGES:CNB_DAILY_EXRATE_URL"] == null)
            {
                var errUrlNotSet = "EXCHANGES:CNB_DAILY_EXRATE_URL variable is not set";
                _logger.LogCritical(errUrlNotSet);
                throw new InvalidOperationException(errUrlNotSet);
            }

            HttpResponseMessage response;

            try
            {
                response = await _httpResilientClient.DoGet(_baseUrl + dailyExRateUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            if(!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to retrieve CNB exchange rates. Status code: " + response.StatusCode);
                throw new InvalidOperationException(response.StatusCode.ToString());
            }

            var responseString = await response.Content.ReadAsStringAsync();

            IEnumerable<CnbRate> cnbRates;
            try
            {
                cnbRates = CnbDailyRateResponseMapper.MapToExchangeRates(responseString);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            var selectedRates = new List<ExchangeRate>();
            var targetCurrency = new Currency(_config["EXCHANGES:CNB_TARGET_CURRENCY"]);

            foreach (var cnbRate in cnbRates)
            {
                if(currencies.Select(x => x.Code).Contains(cnbRate.CurrencyCode))
                {
                    var srcCurrency = new Currency(cnbRate.CurrencyCode);
                    var val = cnbRate.RateVal / cnbRate.Amount;
                    selectedRates.Add(new ExchangeRate(srcCurrency, targetCurrency, val));
                }
            }

            return selectedRates;
        }
    }
}
