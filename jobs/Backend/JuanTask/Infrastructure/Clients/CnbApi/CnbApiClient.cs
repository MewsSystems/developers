using Infrastructure.Clients.CnbApi.Basetypes;
using Infrastructure.Extensions;
using System.Net.Http.Json;

namespace Infrastructure.Clients.CnbApi
{
    public class CnbApiClient : ICnbApiClient
    {

        private readonly HttpClient _httpClient;

        public CnbApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<RatesResponse> GetExchangeRatesDaily(DateTimeOffset dateTime)
        {
            return _httpClient.GetFromJsonAsync<RatesResponse>($"/cnbapi/exrates/daily?date={dateTime.ToPragueDateTime():yyyy-MM-dd}&lang=EN");
        }
    }
}
