using System.Collections.Generic;
using System.Linq;
using Oanda;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> toReturn = new List<ExchangeRate>();
            ExchangeRates ER = new ExchangeRates("OBsfZ4UDsAa3IVBByphPgTVY");
            foreach (var sourceCurr in currencies)
            {
                foreach (var targetCurr in currencies)
                {
                    if (sourceCurr == targetCurr)                   // faster then ask to server
                    {
                        toReturn.Add(new ExchangeRate(sourceCurr, targetCurr, 1));
                        continue;
                    }
                    var response = ER.GetRates(sourceCurr.Code, targetCurr.Code, ExchangeRates.RatesFields.Averages);
                    if (response.IsSuccessful)                                 // ignoring rates, which are not specified by source
                    {
                        decimal middlePrice = ((decimal)(response.Quotes.First().Value.Ask + response.Quotes.First().Value.Bid)) / 2;       // to get average
                        toReturn.Add(new ExchangeRate(sourceCurr, targetCurr, middlePrice));
                    }
                }
            }
            return toReturn;
        }
    }
}
