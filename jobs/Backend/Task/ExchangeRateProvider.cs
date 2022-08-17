using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExchangeRateGetter;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            /*General assumptions:
                - the currencies supplied are the ones where conversion is expected to CZK
                - Implementing for a specific URL even though it could be implemented logic to check multiple places
             */

            var _exchangeRateGetter = new CnbExchangeRateGetter();

            var loadedCurrencies = await _exchangeRateGetter.GetTodaysExchangeRates();

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var currencyData = loadedCurrencies.FirstOrDefault(entry => entry.Currency.Code.ToLower() == currency.Code.ToLower()); //to reduce type chances

                if (currencyData != null)
                {
                    exchangeRates.Add(new ExchangeRate(currencyData.Currency, new Currency("CZK"), (decimal)currencyData.Rate / currencyData.Factor));
                }
            }

            return exchangeRates;
        }
    }
}
