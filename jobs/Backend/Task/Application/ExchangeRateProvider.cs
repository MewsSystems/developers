using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure.Service;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger _logger;

        public ExchangeRateProvider(IExchangeRateService exchangeRateService, ILogger logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                var response = await _exchangeRateService.GetDailyExchangeRates(CancellationToken.None);
                return response.Rates
                    .Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate))
                    .Where(x => currencies.Any(y => y.Code == x.SourceCurrency.Code));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error when filtering exchange rates.");
                throw;
            }
        }
    }
}