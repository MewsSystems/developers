using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class CnbClient
    {

        private const string API_URL_FORMAT = @"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        public static string GetCnbRates()
        {
            // Download content from CNB
            using (var wc = new WebClient())
            {
                return  wc.DownloadString(API_URL_FORMAT);
            }
        }

    }
}
