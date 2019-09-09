using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;


namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {      
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            
            var xmldoc = new XmlDocument();
            xmldoc.Load(@"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

            XmlNodeList nodes = xmldoc.SelectNodes("//*[@kod]");


            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var rate = new ExchangeRate(new Currency("CZK"), new Currency(node.Attributes["kod"].Value),
                        Decimal.Parse(node.Attributes["kurz"].Value,NumberStyles.Any,
                            new CultureInfo("cs-CZ")));

                    yield return rate;
                } 
            }
            
        }

    }
}
