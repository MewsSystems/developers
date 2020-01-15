using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public class XmlExchangeRateLoader : IExchangeRateLoader
    {
        private readonly Uri _exchangeRateEndpoint;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public XmlExchangeRateLoader(Uri exchangeRateEndpoint, ILogger logger)
        {
            _exchangeRateEndpoint = exchangeRateEndpoint
                ?? throw new ArgumentNullException(nameof(exchangeRateEndpoint));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = new HttpClient();
        }

        public async Task<IReadOnlyCollection<ExchangeRate>> LoadExchangeRates()
        {
            var response = await _httpClient.GetAsync(_exchangeRateEndpoint)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _logger.Log(string.Format(
                    CultureInfo.InvariantCulture,
                    "Exchange rate source returned status {0} with response: {1}",
                    response.StatusCode,
                    content));

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
