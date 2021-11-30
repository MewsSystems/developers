using System;
using ExchangeRateUpdater.Interfaces;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ExchangeRateUpdater.Entities.Xml;

namespace ExchangeRateUpdater.BusinessLayer
{

    /// <summary>
    /// Source for the CNB Rates list for main currencies
    /// </summary>
    public class CnbWebXmlRatesSource : ICnbXmlSource
    {
        private string XmlUrlConfigKey => "CnbRatesXMlUrl";


        protected string GetXmlUrl(string key)
        {
             if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
             {
                 throw new ConfigurationErrorsException($"{key} is  missing from AppSettings.config");
             }

             return ConfigurationManager.AppSettings[key];
        }

        public virtual async Task<CnbXmlRatesDocument> GetRates()
        {
            return await GetRates(GetXmlUrl(XmlUrlConfigKey));
        }

        protected async Task<CnbXmlRatesDocument> GetRates(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var stream = await client.GetStreamAsync(url);
                    var serializer = new XmlSerializer(typeof(CnbXmlRatesDocument), new XmlRootAttribute("kurzy"));
                    return (serializer.Deserialize(stream) as CnbXmlRatesDocument);
                }

            }
            catch (Exception e)
            {
                throw new SerializationException("CNB Rates Web Xml source Exception", e);
            }

        }
    }
}
