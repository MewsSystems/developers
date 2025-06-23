using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClient _httpClient;
        private readonly IExchangeRateParser _exchangeRateParser;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly string _url;

        public ExchangeRateProvider(IHttpClient httpClient, IExchangeRateParser parser, IConfiguration configuration, ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _exchangeRateParser = parser;
            _url = configuration["ExchangeRateProvider:Url"];
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();
            try
            {
                var today = DateTime.Now.ToString("yyyy-MM-dd");
                var requestUrl = string.Format(_url, today);

                var response = await _httpClient.GetStringAsync(requestUrl);
                return _exchangeRateParser.Parse(response, currencies);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching data: {ex.Message}");
            }

            return exchangeRates;
        }
    }
}
