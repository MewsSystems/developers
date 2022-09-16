using CzechNationalBankAPI.Model;
using Model;
using Model.Entities;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace CzechNationalBankGateway
{
    public static class ExchangeRateGatewayCNB
    {
         const string CNB_EXCHANGE_RATE_URL = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        const string DEFAULT_CURRENCY = "CZK";

        private static async Task<ExchangeRatesCNB?> GetExchangeRatesFromCNBAsync()
        {
            using var client = new HttpClient();
            using var responseStream = await client.GetStreamAsync(CNB_EXCHANGE_RATE_URL);
            var serializer = new XmlSerializer(typeof(ExchangeRatesCNB));

            return (ExchangeRatesCNB?)serializer.Deserialize(responseStream);
        }

        public static async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            var exchangeRates = await GetExchangeRatesFromCNBAsync();
            if (exchangeRates != null)
            {
                return exchangeRates.Table.Row
                  .Where(x => x.Rate != null)
                  .Select(x => new ExchangeRate(
                    new Currency(x.Code),
                    new Currency(DEFAULT_CURRENCY),
                    x.Rate.Value));
            }

            return Enumerable.Empty<ExchangeRate>();
        }
    }
}