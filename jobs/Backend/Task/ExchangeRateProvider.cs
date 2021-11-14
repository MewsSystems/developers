using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private IEnumerable<IExchangeRatesSource> ExchangeRatesSources { get; set; }

        public ExchangeRateProvider()
        {
            ExchangeRatesSources = new List<IExchangeRatesSource>
            {
                new CnbDailyExchangeRatesSource(),
                new CnbMontlyExchangeRatesSource()
            };
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            /* First of all, I would search through interet, if someone havent already done, what I am trying to do.
             * https://www.altair.blog/2016/08/zpracovani-aktualnich-a-historickych-kurzu-men-pres-api-cnb
               and it seems that this code, has been already writen by altair. I will no use it.
               I would not use it, at leat now, but when I finish, I will compare my code to altair. */

            /* I don't like his solution that much, but he used a few techniques that I forgot or didn't even know,
             * but I don't want to use it now, because I want to show my original solution
             * (async await - I didn't realize that DownloadString also has an async version
             * and I was surprised how string.Format method was used here) */

            /* CNB seems to have some APIs, but they are not helpful https://www.api.store/cnb.cz/ */

            var pairs = GetCurrencyPairs(currencies.ToList());

            return GetExchangeRates(pairs);
        }

        private static IEnumerable<ExchangePair> GetCurrencyPairs(IList<Currency> currencies)
        {
            foreach (var firstCurrency in currencies)
            {
                foreach (var secondCurrency in currencies)
                {
                    if (firstCurrency.Code == secondCurrency.Code)
                        continue;

                    yield return new ExchangePair(firstCurrency, secondCurrency);
                }
            }
        }

        private IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<ExchangePair> pairs)
        {
            foreach (var pair in pairs)
            {
                bool found = false;
                foreach (var exchangeRateLoader in ExchangeRatesSources)
                {
                    if (found)
                        continue;

                    var exchangeRate = exchangeRateLoader.Get(pair);

                    if (exchangeRate == null)
                        continue;

                    found = true;

                    yield return exchangeRate;
                }
            }
        }
    }
}