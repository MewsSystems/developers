using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Globalization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string XmlDataSourceFilePath = @"https://www.csob.cz/portal/lide/produkty/kurzovni-listky/kurzovni-listek/-/date/2017-06-18/kurzovni-listek.xml";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            
            _GetXMLData(exchangeRates, _GetTempFile());

            return _FillRatesByCurrencies(currencies, exchangeRates);
        }
        /// <summary>
        /// Fills the list of exchange rates according to the list of currencies
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="exchangeRates"></param>
        /// <returns></returns>
        private List<ExchangeRate> _FillRatesByCurrencies(IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRates)
        {
            List<ExchangeRate> ratesByCurrencies = new List<ExchangeRate>();
            foreach (Currency currency in currencies)
            {
                foreach (ExchangeRate exRate in exchangeRates)
                {
                    if (exRate.SourceCurrency.Code == currency.Code || exRate.TargetCurrency.Code == currency.Code)
                    {
                        if (!ratesByCurrencies.Contains(exRate))
                        {
                            ratesByCurrencies.Add(exRate);
                        }
                    }
                }
            }
            return ratesByCurrencies;
        }
        /// <summary>
        /// Downloads the XML file to a temporary file 
        /// </summary>
        /// <returns>Full path to temp file</returns>
        private string _GetTempFile()
        {
            string xmlDataTempFileName = Path.GetTempFileName();
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new System.Uri(XmlDataSourceFilePath), xmlDataTempFileName);
            }
            return xmlDataTempFileName;
        }
        /// <summary>
        /// Gets XML data from temporary file and fills list of exchange rates
        /// </summary>
        /// <param name="exchangeRates"></param>
        /// <param name="xmlDataTempFileName"></param>
        private void _GetXMLData(List<ExchangeRate> exchangeRates, string xmlDataTempFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlDataTempFileName);
           
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
            {
                string curr = xmlNode.Attributes["ID"].Value;
                string qt = xmlNode.Attributes["quota"].Value;
                string val = xmlNode.FirstChild.Attributes["Middle"].Value;

                if (!string.IsNullOrEmpty(curr) && !string.IsNullOrEmpty(qt) && !string.IsNullOrEmpty(val))
                {
                    decimal quota = decimal.Parse(qt, NumberStyles.Currency, CultureInfo.InvariantCulture);
                    decimal value = decimal.Parse(val, NumberStyles.Currency, CultureInfo.InvariantCulture);

                    exchangeRates.Add(new ExchangeRate(new Currency(curr), new Currency("CZK"), value / quota)); //In this case, target currency is always CZK
                }
            }
        }
    }
}
