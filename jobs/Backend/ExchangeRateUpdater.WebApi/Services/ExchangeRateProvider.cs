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
