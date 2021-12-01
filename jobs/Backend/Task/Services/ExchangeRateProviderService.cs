using ExchangeRateUpdater.Services.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private string _exchangeRateUrl = ConfigurationManager.AppSettings["ExchangeRateURL"];

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetCNBExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var cnbResult = await SendGetExchangeRatesRequestAsync(_exchangeRateUrl);

            var deserializedResult = ExchangeRateDeserializer.DeserializeCNBExchangeRateString(cnbResult);

            var result = deserializedResult.Where(x => currencies.Contains(x.TargetCurrency));

            return result;
        }

        private async Task<string> SendGetExchangeRatesRequestAsync(string url)
        {
            var result = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                result = await httpClient.GetStringAsync(url);
            }

            return result;
        }
    }
}
