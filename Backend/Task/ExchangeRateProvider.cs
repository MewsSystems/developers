using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        IRateDataProvider dataProvider { get; set; }
        public ExchangeRateProvider()
        {
            dataProvider = new CNBRateDataProvider(); // normally would use dependency injection, but there is none in program and i dont want to break it
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                yield break;
            }

            var cnbData = dataProvider.Data;

            foreach (var toCurrency in currencies)
            {
                if (cnbData.ContainsKey(toCurrency.Code))
                    yield return Rate(dataProvider.ProvidedFor, cnbData[toCurrency.Code]);
            }

            yield break;
        }

        public ExchangeRate Rate(CurrencyRate from, CurrencyRate to)
        {
            return new ExchangeRate(from.Currency, to.Currency, from.Rate * from.Amount / to.Rate * to.Amount);
        }
    }
}
