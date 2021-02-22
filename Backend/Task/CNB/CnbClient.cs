using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.CNB
{
    public class CnbClient : ICnbClient
    {
        public const string EXCHANGE_RATES_URL = @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?ref=public-apis";

        private readonly Lazy<HttpClient> client = new Lazy<HttpClient>(() => new HttpClient());
        private readonly ICnbResponseParser parser;

        public CnbClient(ICnbResponseParser parser)
        {
            this.parser = parser;
        }

        public ExchangeRatesCollectionDto GetExchangeRates()
        {
            var request = client.Value.GetAsync(EXCHANGE_RATES_URL);
            request.Wait();
            var response = request.Result;

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStreamAsync();
            content.Wait();
            using (var contentStream = content.Result)
            {
                return parser.Parse(contentStream);
            }
        }
    }

    public interface ICnbClient
    {
        ExchangeRatesCollectionDto GetExchangeRates();
    }
}
