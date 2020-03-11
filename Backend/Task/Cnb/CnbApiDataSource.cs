using System.Net.Http;

namespace ExchangeRateUpdater.Cnb
{
    public class CnbApiDataSource
    {
        public string FetchRatesData()
        {
            using (HttpClient client = new HttpClient())
            {
                return client
                    .GetStringAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt")
                    .Result;
            }
        }
    }
}