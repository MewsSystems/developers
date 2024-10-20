using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Provider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        readonly IExchangeRateClient _exchangeRateClient;
        readonly ILogger _logger;

        public ExchangeRateProvider(IExchangeRateClient exchangeRateClient, ILogger<ExchangeRateProvider> logger) 
        { 
            _exchangeRateClient = exchangeRateClient;
            _logger = logger;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var exchangeRateEntities = Enumerable.Empty<ExchangeRateEntity>();

            try
            {
                exchangeRateEntities = await _exchangeRateClient.GetExchangeRateEntitiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while calling the client: {ex.GetType()}: {ex.Message}");
                return new List<ExchangeRate>();
            }

            if (exchangeRateEntities == null || !exchangeRateEntities.Any())
            {
				_logger.LogWarning("No exchange rates were received from the client");

				return new List<ExchangeRate>();
            }

            var targetCurrency = new Currency(ExchangeRateSettings.TargetCurrency);

            var currencyCodes = currencies.Select(c => c.Code).ToHashSet();

            var result = exchangeRateEntities
				.Where(e => currencyCodes.Contains(e.CurrencyCode))
                .Select(e => new ExchangeRate(new Currency(e.CurrencyCode), targetCurrency, e.Rate))
                .ToList();

            _logger.LogInformation($"{result.Count} exchange rates were returned by provider from {exchangeRateEntities.ToList().Count} entities received from the client");
            
            return result;
        }
    }
}
