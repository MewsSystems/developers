using System.Net;

namespace ExchangeRateUpdater
{
    public class CnbRateClient
    {
        private readonly string SourceUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        public virtual string GetRatesDataFromSource()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(SourceUrl);
            }
        }
    }
}
