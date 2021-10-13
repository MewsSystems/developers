using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data
{
    public class CNBConnection
    {
        public const string ExchangeRateXml = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        public async static Task<string> GetXml(string xmlendpoint)
        {
            HttpClient client = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            string response = await client.GetStringAsync(xmlendpoint);

            return response;
        }
    }
}
