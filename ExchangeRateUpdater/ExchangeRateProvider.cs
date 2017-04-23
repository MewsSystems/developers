using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        const string URLAddress = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        //path to file containing raw data
        string tempFileName;

        private List<ExchangeRate> exchangeRates { get; set; }

        public ExchangeRateProvider()
        {
            exchangeRates = new List<ExchangeRate>();
        }

        /// <summary>
        /// Downloads file containing exchange rates to temp file.
        /// </summary>
        /// <returns><c>true</c>, if file has been successfully downloaded, <c>false</c> otherwise.</returns>
        private void downloadSourceFile()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(new Uri(URLAddress), tempFileName = Path.GetTempFileName());
            }
        }

        /// <summary>
        /// Extracts exchange rates from .txt file. Format is defined by cnb.cz. (
        /// </summary>
        private void extractExchangeRateData()
        {
            using (StreamReader sr = new StreamReader(tempFileName))
            {
                sr.ReadLine(); //omiting useless first line in file
                sr.ReadLine(); //omiting useless second line in file

                List<string> columns;
                ExchangeRate exchangeRate;
                decimal value, coeficient;

                while (!sr.EndOfStream)
                {
                    columns = new List<string>(sr.ReadLine().Split('|'));
                    Currency currency;

                    //parsing exchange rate value and coeficient
                    if (decimal.TryParse(columns[4].Replace(',', '.'), out value) && decimal.TryParse(columns[2].Replace(',', '.'), out coeficient))
                    {
                        currency = new Currency(columns[3]);
                        exchangeRate = new ExchangeRate(currency, new Currency("CZK"), value / coeficient);
                        exchangeRates.Add(exchangeRate);
                    }
                }
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (exchangeRates.Count == 0)
            {
                downloadSourceFile();
                extractExchangeRateData();
            }
      
            //used to return unique items
            HashSet<ExchangeRate> toReturn = new HashSet<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var currenciesToReturn = 
                    from rate in exchangeRates 
                        where rate.SourceCurrency.Code.Equals(currency.Code) || rate.TargetCurrency.Code.Equals(currency.Code) 
                    select rate;
                
                toReturn.UnionWith(currenciesToReturn);
            }
            return toReturn;

        }
    }
}