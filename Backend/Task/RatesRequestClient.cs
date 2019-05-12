using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ExchangeRateUpdater
{
    class RatesRequestClient
    {
        private static HttpClient client;
        private const string CacheName = "CurrencyRates";

        public ExchangeRateResponse GetRatesData(RateRequestModel req, CancellationToken cancelToken = default(CancellationToken))
        {
            string fullUrl = req.RequestUrl + req.Date;
            ExchangeRateResponse ratesResponse = null;
            try
            {
                using (client = new HttpClient())
                {
                    var response = client.GetAsync(fullUrl).Result;
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseXML = response.Content.ReadAsStringAsync().Result;
                        var bytes = Encoding.UTF8.GetBytes(responseXML);
                        var stream = new MemoryStream(bytes);
                        XmlSerializer _serializer = new XmlSerializer(typeof(ExchangeRateResponse));
                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            ratesResponse = (ExchangeRateResponse)_serializer.Deserialize(reader);
                            reader.Close();
                        }     
                        return ratesResponse;
                    }
                    
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine("Exception: " + e.Message);
            }      
            return null;
        }
    }
}
