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
    public class TextExchangeRateProvider : ITextExchangeRateProvider
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IExchangeRateParser _exchangeRateParser;
        private IConfiguration _configuration;
        private readonly ILogger _logger;

        public TextExchangeRateProvider(IHttpClientService httpClientService, IExchangeRateParser exchangeRateParser, IConfiguration configuration, ILogger<TextExchangeRateProvider> logger)
        {
            _httpClientService = httpClientService;
            _exchangeRateParser = exchangeRateParser;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime? date)
        {
            try
            {
                // If a date is provided, append it to the URL.
                var exchangeRateUrl = _configuration["ExchangeRateUrl"];
                if (date.HasValue)
                {
                    var formattedDate = date.Value.ToString("dd.MM.yyyy");
                    exchangeRateUrl += $"?date={formattedDate}";
                }

                var response = await _httpClientService.GetStringAsync(exchangeRateUrl);
                var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                var rates = _exchangeRateParser.ParseRatesFromText(currencies, lines);

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetExchangeRatesTextAsync");
                throw;
            }
        }

    }

}
