using System;
using System.Collections;
using System.Configuration;

namespace ExchangeRateUpdater.CnbProxy.Configuration
{
    public class CnbConfiguration : ICnbConfiguration
    {
        private const string sectionName = "cnb";
        private const string xmlUrl = "xmlUrl";

        public CnbConfiguration()
        {
            var oneTag = (IDictionary)ConfigurationManager.GetSection(sectionName);
            UrlToXmlExchangeRates = (string)oneTag[xmlUrl];
        }
 
        public string UrlToXmlExchangeRates { get; }
    }
}
