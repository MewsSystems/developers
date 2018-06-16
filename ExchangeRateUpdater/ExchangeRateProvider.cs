using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        //Since the operation is done in main we can afford to make this url static without worrying about the date
        private static string exchangeRatesUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=" + DateTime.Now.ToString("dd.MM.yyy");

        private static string defaultCurrency = "CZK";

        private static string[] lineSplitter = new string[] { "\r\n", "\r", "\n" };

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return ParseCnbCurrencies(DownloadCnbCurrencies(), currencies);
        }

        /// <summary>
        /// Download the currencies from <see cref="exchangeRatesUrl"/>
        /// </summary>
        /// <returns>downloaded data as string</returns>
        private string DownloadCnbCurrencies()
        {
            string response = String.Empty;
            using (var webClient = new System.Net.WebClient())
            {
                response = webClient.DownloadString(exchangeRatesUrl);
            }
            return response;
        }

        /// <summary>
        /// Parses <param name="response"> to get the rates and restricts them by provided <param name="currencies">
        /// </summary>
        /// <param name="response">downloaded data from ČNB</param>
        /// <param name="currencies">currencies for which exchageRates are requested</param>
        /// <returns>List of <see cref="ExchangeRate"/></returns>
        private IEnumerable<ExchangeRate> ParseCnbCurrencies(string response, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();

            //If there are no requested currencies we just return empty list
            if (currencies == null || currencies.Count() == 0)
                return result;

            IEnumerable<string> lines = response.Split( lineSplitter, StringSplitOptions.RemoveEmptyEntries ).Skip(2);
           
            foreach(string line in lines)
            {
                string[] rowData = line.Split('|');
                string currencyCode = rowData[3];
                string exchangeRate = rowData[4];

                if (!currencies.Any(p => p.Code == currencyCode))
                    continue;
                
                result.Add(new ExchangeRate(new Currency(defaultCurrency), new Currency(currencyCode), Convert.ToDecimal(exchangeRate)));
            }
            
            return result;
        }
    }
}
