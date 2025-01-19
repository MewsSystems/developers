using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using ExchangeRateUpdater.Domain.Models;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Services;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.DTO;
namespace ExchangeRateUpdater.Providers
{
    public class ExchangeRateProvider(ILogger<ExchangeRateProvider> _logger, IExchangeRateService exchangeRateService) : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRateDTO>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRateDTO>();
            try
            {
                _logger.LogInformation("Started fetching exchange rates");
                var exchangeRates = await exchangeRateService.GetExchangeRateAsync();
                foreach (var currency in currencies)
                {
                    _logger.LogInformation($"Adding exchange rate for {currency.Code}");
                    result.AddRange(exchangeRates.Rates.Where(e => e.CurrencyCode == currency.Code));
                    _logger.LogInformation($"Added exchange rate for {currency.Code}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return result;
            }
            return result;
        }
    }
}
