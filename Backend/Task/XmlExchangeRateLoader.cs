using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public class XmlExchangeRateLoader : IExchangeRateLoader
    {
        private readonly Uri _exchangeRateEndpoint;
        private readonly HttpClient _httpClient;

        public XmlExchangeRateLoader(Uri exchangeRateEndpoint)
        {
            _exchangeRateEndpoint = exchangeRateEndpoint
                ?? throw new ArgumentNullException(nameof(exchangeRateEndpoint));
            _httpClient = new HttpClient();
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> LoadExchangeRates()
        {
            var response = await _httpClient.GetAsync(_exchangeRateEndpoint)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                // log the response
                return new List<ExchangeRate>();
            }

            var exchangeRates = await ParseExchangeRatesFromResponse(response.Content)
                .ConfigureAwait(false);

            return exchangeRates;
        }

        private async Task<IReadOnlyCollection<ExchangeRate>> ParseExchangeRatesFromResponse(HttpContent response)
        {
            var xmlStream = await response.ReadAsStreamAsync()
                .ConfigureAwait(false);

            var document = XDocument.Load(xmlStream);

            return document.Descendants("radek")
                .Select(e =>
                {
                    var code = e.Attribute("kod").Value;
                    var exchangeRate = e.Attribute("kurz").Value;

                    if (decimal.TryParse(exchangeRate, out var value))
                    {
                        return new ExchangeRate(Currency.Czech, new Currency(code), value);
                    }

                    return null;
                })
                .Where(er => er != null)
                .ToList();
        }
    }
}
