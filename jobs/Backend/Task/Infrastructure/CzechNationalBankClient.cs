using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application;

namespace ExchangeRateUpdater.Infrastructure
{
    public class CzechNationalBankClient : ICzechNationalBankClient
    {
        private readonly HttpClient _httpClient;

        public CzechNationalBankClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CnbExchangeRateResponse> GetExchangeRatesAsync(CancellationToken cancellationToken)
        {
            var rates = await _httpClient.GetFromJsonAsync<CnbExchangeRateResponse>("exrates/daily", cancellationToken);
            return rates ?? throw new HttpRequestException("Exchange rates are null");
        }
    }
}
