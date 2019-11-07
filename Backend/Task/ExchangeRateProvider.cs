using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml;

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
             var xmlPath = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

            using (XmlTextReader reader = new XmlTextReader(xmlPath))
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                XmlNodeList xmlCurrency = document.GetElementsByTagName("radek");
                foreach (XmlNode item in xmlCurrency)
                {
                    var currency = currencies.FirstOrDefault(o => o.Code == item.Attributes["kod"].InnerText);
                    if (currency != null)
                    {
                        yield return new ExchangeRate(currency, new Currency("CZK"), decimal.Parse(item.Attributes["kurz"].InnerText));
                    }
                }
            }
        }
    }
}
