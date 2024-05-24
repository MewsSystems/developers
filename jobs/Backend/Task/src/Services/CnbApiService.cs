using ExchangeRateUpdater.Models;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Services
{
    public class CnbApiService : ICnbApiService
    {
        private readonly string GET_RATES_ENDPOINT = "/cnbapi/exrates/daily";

        private readonly HttpClient _client;

        public CnbApiService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("CnbClient");
        }

        public async Task<CnbRateDailyResponse> GetExchangeRate(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(GET_RATES_ENDPOINT, UriKind.Relative)
            };

            var httpResponse = await _client.SendAsync(httpRequest);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new CnbRateDailyResponse
                {
                    Rates = new System.Collections.Generic.List<CnbRate>()
                };
            }

            var response = await httpResponse.Content.ReadAsStringAsync();

            var cnbRates = JsonConvert.DeserializeObject<CnbRateDailyResponse>(response);

            return cnbRates;
        }
    }
}