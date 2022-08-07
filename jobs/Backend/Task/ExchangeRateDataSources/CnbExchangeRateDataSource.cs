using ExchangeRateUpdater.ExchangeRateDataProviders;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateDataSources
{
    public class CnbExchangeRateDataSource : IExchangeRateDataSource
    {
        private readonly HttpClient httpClient;
        private const string url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        public CnbExchangeRateDataSource(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
            this.httpClient = httpClient;
        }

        public async Task<Stream> GetDataAsync(CancellationToken ct = default)
        {
            var responce = await httpClient.GetAsync(url, ct);

            if (!responce.IsSuccessStatusCode && responce.Content != null)
            {
                throw new Exception($"Failed to get data from CNB, status code {responce.StatusCode}");
            }

            var stream = await responce.Content.ReadAsStreamAsync(ct);

            return stream;
        }
    }
}
