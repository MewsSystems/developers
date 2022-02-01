using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;  // Enables parsing of xml data (web page source)

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/index.html?date=15.01.2022";
            string table = GetHtmlTable(url);

            List<ExchangeRate> exRates = new List<ExchangeRate>();  // empty List to hold found exchange rates
            XDocument xDoc = XDocument.Parse(table);                // XDocument to hold the information from the table

            foreach (XElement xElem in xDoc.Descendants("tr"))
            {
                IList<XElement> indexedElems = xElem.Elements().ToList();   // convert XElems to List for easy access via index

                //Insert exchange rates to exRates in 1-to-1 format
                if (currencies.Any(x => x.Code == indexedElems[3].Value))
                {
                    Currency srcCurr = new Currency(indexedElems[3].Value); // pull the source currency's ISO-4217 code
                    Currency trgCurr = new Currency("CZK");                 // could be any code but from this url it makes sense

                    int srcAmount = int.Parse(indexedElems[2].Value);       // source amount will be whole
                    decimal trgRate = decimal.Parse(indexedElems[4].Value); // url gives target currency by rate, not amount

                    // ensures rate is in 1-to-1 basis, eg. some exchanges are 100-to-1
                    decimal exRate = srcAmount == 1 ? trgRate : decimal.Divide(trgRate, srcAmount); 

                    exRates.Add(new ExchangeRate(srcCurr, trgCurr, exRate));    // add rates to the list to be returned
                }
            }

            return exRates;
        }

        /// <summary>
        /// Gets the first HTML table from a web page. 
        /// Improvements: 
        ///     - Get a specified table, or simply all tables as List<string> (depending on web page)
        /// </summary>
        private string GetHtmlTable(string url)
        {
            var webclient = new System.Net.WebClient();     // WebClient downloads web pages and files
            string content = webclient.DownloadString(url); // download the web page (via url) to "content"
            
            string tableStart = "<table";   // HTML table begin-tag (not whole tag)
            string tableEnd = "</table>";   // HTML table end-tag

            int index1 = content.IndexOf(tableStart);                       // index of begin-tag
            int index2 = content.IndexOf(tableEnd) + tableEnd.Length;       // index of end of end-tag

            string firstTable = content.Substring(index1, index2 - index1); // Just the table from the web page

            return firstTable;
        }
    }
}
