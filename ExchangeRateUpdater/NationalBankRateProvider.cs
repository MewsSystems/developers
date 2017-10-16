using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    public static class NationalBankRateProvider
    {
        public static IEnumerable<BankRate> GetBankRates()
        {
            var url = ConfigurationManager.AppSettings["SourceUrl"];

            var request = (HttpWebRequest)WebRequest.Create(url);
            var serializer = new XmlSerializer(typeof(BankRatesInformation));

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = Encoding.GetEncoding(response.CharacterSet);

                    try
                    {
                        using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            return ((BankRatesInformation)serializer.Deserialize(reader)).Table.Rates;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Deserialization from XML failed with error: '{ex.Message}'");
                    }
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
    }
}
