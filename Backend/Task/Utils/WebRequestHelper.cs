using System;
using System.Net;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Utils
{
    public static class WebRequestHelper
    {
        public static T GetXmlRequest<T>(Uri requestUrl)
        {
            HttpWebResponse response = null;

            try
            {
                var httpRequest = (HttpWebRequest) WebRequest.Create(requestUrl.AbsoluteUri);
                response = (HttpWebResponse) httpRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            return default(T);
                        
                        var xmlSerializer = new XmlSerializer(typeof(T));
                        return (T) xmlSerializer.Deserialize(stream);
                    }
                }

                Console.WriteLine($"Host '{requestUrl.Host}' returnted status code '{response.StatusCode}'.");
                return default(T);
            }
            catch (WebException ex)
            {
                using (var exResponse = ex.Response)
                {
                    var webReponse = ((HttpWebResponse) exResponse);
                    
                    Console.WriteLine($"Host '{requestUrl.Host}' is not responding (status code: {webReponse.StatusCode}, message: {ex.Message}).");
                    return default(T);
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Couldn't deserialize XML '{typeof(T)}' from host '{requestUrl.Host}'");
                return default(T);
            }
            finally
            {
                response?.Close();
            }
        }        
    }
}