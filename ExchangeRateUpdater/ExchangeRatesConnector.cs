using System.IO;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
    public interface IExchangeRatesConnector
    {
        string GetDayRatesInRawFormat();
    }

    internal class ExchangeRatesConnector : IExchangeRatesConnector
    {
        public string GetDayRatesInRawFormat()
        {
            HttpWebRequest fileReq = (HttpWebRequest)WebRequest.Create("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt");
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            if (fileResp.ContentLength > 0)
            {
                Stream stream = fileResp.GetResponseStream();

                if (stream != null)
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string result = reader.ReadToEnd();
                    reader.Close();

                    return result;
                }
            }

            return string.Empty;
        }
    }
}
