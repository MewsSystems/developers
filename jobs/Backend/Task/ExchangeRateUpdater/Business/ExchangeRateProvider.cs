using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Business.Interfaces;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Business
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly IForeignExchangeService _foreignExchangeService;

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IForeignExchangeService foreignExchangeService)
        {
            _logger = logger;
            _foreignExchangeService = foreignExchangeService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
        {
            _logger.LogDebug($"Entering {nameof(GetExchangeRatesAsync)}");

            // Request rates from a third party API
            var thirdPartyRates = await _foreignExchangeService.GetLiveRatesAsync();

            // Convert response to ExchangeRate
            if (thirdPartyRates != null)
            {
                var sourceCurrency = new Currency("CZK");
                var exchangeRates = ExchangeRateHelper.ConvertToExchangeRates(thirdPartyRates, sourceCurrency);

                _logger.LogDebug($"Returning {exchangeRates.Count} exchange rates");
                return exchangeRates;
            }

            return null;
        }
    }
}
