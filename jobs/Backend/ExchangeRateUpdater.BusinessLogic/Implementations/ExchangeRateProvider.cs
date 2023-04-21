using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.BusinessLogic.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        public readonly IExchangesServicesFactory _exchangeServicesFactory;

        public ExchangeRateProvider(IExchangesServicesFactory exchangeServicesFactory)
        {
            _exchangeServicesFactory = exchangeServicesFactory;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, Currency targetCurrency)
        {
            return _exchangeServicesFactory.GetExchangeService(targetCurrency).GetExchangeRates(currencies);
        }
    }
}
