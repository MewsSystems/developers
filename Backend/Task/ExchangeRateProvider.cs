using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

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
        /// 
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = new List<ExchangeRate>() { };
            result.AddRange(ParseCnb(currencies));
            
            return result;
        }

        private readonly string CnbRatesUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        private List<ExchangeRate> ParseCnb(IEnumerable<Currency> currencies, DateTime? dateTime = null) {
            if (dateTime == null)
            {
                dateTime = DateTime.Now;
            }

            // dateTime is now always defined, so we can use ToString method
            string responseFromServer = LoadData(CnbRatesUrl + "?date=" + ((DateTime)dateTime).ToString("dd.MM.YYYY"));

            var resultExchangeRates = new List<ExchangeRate>();

            if (responseFromServer != null)
            {
                // first 2 rows are generated number and headers = we have to skip it
                var lines = responseFromServer.Split('\n').Skip(2).ToList();

                Currency czkCurrency = currencies.Where(x => x.Code == "CZK").FirstOrDefault();

                // set culture, because of parsing of number
                var culture = new CultureInfo("en-US");
                for (var i = 0; i < lines.Count(); i++)
                {
                    var lineValues = lines[i].Split('|');
                    if (lineValues.Length != 5)
                    {
                        // unexpected row
                        continue;
                    }

                    // important values 2 (=množství), 3(=kód), 4(=kurz)
                    var activeCurrency = currencies.Where(x => x.Code == lineValues[3]);
                    if (activeCurrency.Count() == 1)
                    {
                        // currency exists, we use this rate
                        resultExchangeRates.Add(
                            new ExchangeRate(
                                activeCurrency.FirstOrDefault(),
                                czkCurrency,
                                Convert.ToDecimal(lineValues[4].Replace(',', '.'), culture) / Convert.ToInt32(lineValues[2])
                                )
                            );
                    }
                }
            }

            return resultExchangeRates;
        }

        private string LoadData(string url)
        {
            try
            {
                WebRequest cnbWebRequest = WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)cnbWebRequest.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                return responseFromServer;
            }
            catch (Exception e)
            {
                Console.WriteLine("Chyba: " + e.Message);
                return null;
            }
        }
    }
}
