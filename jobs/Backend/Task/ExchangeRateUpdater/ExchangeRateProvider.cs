using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly Currency _sourceCurrency = new Currency("CZK"); 
        private ICnbExchangeRateHttpClient _exchangeRateClient;

        public ExchangeRateProvider(ICnbExchangeRateHttpClient exchangeRateClient)
        {
            _exchangeRateClient = exchangeRateClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var allExchangeRates = await _exchangeRateClient.GetTodaysExchangeRates(CancellationToken.None);
            return allExchangeRates
                .Where(x => currencies.Any(curr => curr.Code == x.CurrencyCode))
                .Select(x =>
                    new ExchangeRate(_sourceCurrency, new Currency(x.CurrencyCode), x.ExchangeRate / x.Amount));
        }
    }
}
