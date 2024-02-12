using ExchangeRateUpdater.Infrastructure.CNB;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly CNBService _cnbService;
        private readonly GetDailyRatesResponseHandler _getDailyRatesResponseHandler;
        private readonly ILogger _logger;

        public ExchangeRateProvider(IHttpClientFactory httClientFactory, ILogger logger) 
        {
            _cnbService = new CNBService(httClientFactory, logger);
            _getDailyRatesResponseHandler = new GetDailyRatesResponseHandler();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = await _cnbService.GetExDailyRates();
            return _getDailyRatesResponseHandler.ToExchangeRates(response)
                .Where(r => currencies.Contains(r.SourceCurrency));
        }
    }
}
