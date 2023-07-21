using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Providers
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IApiExchangeRateProvider _apiExchangeRateProvider;
        private readonly ITextExchangeRateProvider _textExchangeRateProvider;
        private readonly ILogger _logger;

        public ExchangeRateProvider(IApiExchangeRateProvider apiExchangeRateProvider, ITextExchangeRateProvider textExchangeRateProvider, ILogger<ExchangeRateProvider> logger)
        {
            _apiExchangeRateProvider = apiExchangeRateProvider;
            _textExchangeRateProvider = textExchangeRateProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime? date)
        {
            CurrencyValidator.ValidateCurrencies(currencies);
            try
            {
                var rates = await _apiExchangeRateProvider.GetExchangeRatesAsync(currencies, date);
                if (!rates.Any())
                {
                    rates = await _textExchangeRateProvider.GetExchangeRatesAsync(currencies, DateTime.Now);
                }

                return rates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetExchangeRatesAsync for currencies: {string.Join(", ", currencies.Select(c => c.Code))} and date: {date}");
                throw;
            }
        }
    }

}
