using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

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

        private const string ExchangeSourceUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?date=";
        private const string AcceptedDateFormat = "dd.MM.yyyy";
        private RatesRequestClient ratesReqClient = null;
        private const string targetCurrency = "CZK";

        public ExchangeRateProvider()
        {
            ratesReqClient = new RatesRequestClient();
        }
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var requiredCodesCur= currencies.Select(c => c.Code).ToList();
            var relevantExcRates = new List <ExchangeRate>();
            Currency comparedCurency = new Currency(targetCurrency);
            var ratesRequest = new RateRequestModel()
            {
                Date = DateTime.Today.ToString(AcceptedDateFormat),
                RequestUrl = ExchangeSourceUrl
            };
            ExchangeRateResponse ratesResponse = ratesReqClient.GetRatesData(ratesRequest);
            if(ratesResponse!=null && ratesResponse.Table.RatesRows.Length > 0)
            {
                foreach (var rate in ratesResponse.Table.RatesRows)
                {
                    if (requiredCodesCur.Contains(rate.Code))
                    {
                        relevantExcRates.Add(new ExchangeRate(new Currency(rate.Code), comparedCurency, (decimal)Convert.ToDecimal(rate.Rate) / Convert.ToInt32(rate.Amount)));

                    }
                    else { continue; }
                }
            }


            return relevantExcRates;
        }
    }
    
}
