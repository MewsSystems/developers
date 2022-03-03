using System;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Util;

namespace ExchangeRateUpdater.CNB.Api
{
    public class CNBExchangeRateClient : IDisposable
    {
        private const string ExchangeRateUri = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        private readonly HttpClient _client = new(new HttpRetryMessageHandler(new HttpClientHandler()));

        public async Task<string> GetExchangeRateXmlAsync()
        {
            using var res = await _client.GetAsync(ExchangeRateUri);
            if (!res.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Request to get the exchange rates from {ExchangeRateUri} failed with status code: {(int)res.StatusCode} - {res.StatusCode}.");
            }
            var xmlText = await res.Content?.ReadAsStringAsync();
            return xmlText;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
