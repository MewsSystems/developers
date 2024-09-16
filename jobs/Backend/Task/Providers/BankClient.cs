using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class BankClient : IBankClient
    {
        private readonly HttpClient _httpClient;

        public BankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetExchangeRatesDto> GetExchangeRatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<GetExchangeRatesDto>("daily")
                            ?? throw new HttpRequestException("Error retrieving the data.");
        }
    }
}
