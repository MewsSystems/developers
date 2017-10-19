using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, DataSourceEnum source = DataSourceEnum.CzechNationalBank)
        {
            var fileDownlo = new FileParser();
            var eRater = new List<ExchangeRate>();
            try
            {
                eRater = fileDownlo.GetDataFromServer(source);
            }
            catch (System.Exception exc)
            {

                throw new System.Exception("Something is terribly wrong with parsing", exc);
            }

            eRater = eRater.Where(er => currencies.Select(c => c.Code).Contains(er.SourceCurrency.Code)).ToList(); //I have no clear idea why, but I need take only Code. Equaling object is tricky.
            return eRater;
        }
    }
}
