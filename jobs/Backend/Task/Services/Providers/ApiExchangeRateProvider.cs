using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExchangeRateUpdater.Services.Providers
{
    public class ApiExchangeRateProvider : IApiExchangeRateProvider
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IExchangeRateParser _exchangeRateParser;
        private IConfiguration _configuration;
        private readonly ILogger _logger;

        public ApiExchangeRateProvider(IHttpClientService httpClientService, IExchangeRateParser exchangeRateParser, IConfiguration configuration, ILogger<ApiExchangeRateProvider> logger)
        {
            _httpClientService = httpClientService;
            _exchangeRateParser = exchangeRateParser;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            try
            {
                string apiUrl = _configuration["ApiUrl"];
                var response = await _httpClientService.GetStringAsync(apiUrl);
                var data = _exchangeRateParser.ParseRatesFromApi(response);
                var rates = _exchangeRateParser.GetRatesFromData(currencies, data);

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetExchangeRatesApiAsync");
                throw new ApiException("An error occurred while trying to retrieve exchange rates.", ex);
            }
        }
    }

}
