using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class ExchangeRateProxy
    {
        public const string serviceUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        private static readonly HttpClient client = new HttpClient();

        public static IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = client.PostAsync(serviceUrl, null);
            var responseString = response.GetAwaiter().GetResult().Content.ReadAsStringAsync();
            var result = ExchangeRateParser.Parse(responseString.GetAwaiter().GetResult());
            return result.Where(rate => currencies.Any(currency => currency.Code == rate.SourceCurrency.Code));
        }
    }
}
