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
        private readonly IExchangeRateProviderFactory _factory;
        private readonly ILogger _logger;

        public ExchangeRateProvider(IExchangeRateProviderFactory factory, ILogger<ExchangeRateProvider> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime? date)
        {
            CurrencyValidator.ValidateCurrencies(currencies);
            try
            {
                var rates = await _factory.Create(ProviderType.api).GetExchangeRatesAsync(currencies, date);

                if (!rates.Any())
                {
                    rates = await _factory.Create(ProviderType.text).GetExchangeRatesAsync(currencies, date);
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
