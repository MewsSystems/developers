using ExchangeRateUpdater.WebApi.Models;
using ExchangeRateUpdater.WebApi.Services.Interfaces;

namespace ExchangeRateUpdater.WebApi.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRatesGetter _exchangeRatesGetter;
        private readonly IExchangeRatesParser _exchangeRatesParser;

        public ExchangeRateProvider(IExchangeRatesGetter exchangeRateGetter, IExchangeRatesParser exchangeRatesParser)
        {
            _exchangeRatesGetter = exchangeRateGetter;
            _exchangeRatesParser = exchangeRatesParser;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = Enumerable.Empty<ExchangeRate>();

            string currentRawExchangeRates = await _exchangeRatesGetter.GetRawExchangeRates();

            IEnumerable<ExchangeRate> currentExchangeRates = _exchangeRatesParser.ParseExchangeRates(currentRawExchangeRates);

            exchangeRates = currentExchangeRates.Where(exchangeRate => currencies.Any(currency => currency.Code == exchangeRate.TargetCurrency.Code));

            return exchangeRates;
        }
    }
}
