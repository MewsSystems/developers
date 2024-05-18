using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Clients
{
    public class CnbApiClient : ICnbApiClient
    {
        private readonly CnbApiClientConfiguration _configuration;
        private readonly HttpClient _client;

        public CnbApiClient(CnbApiClientConfiguration configuration, HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _client.BaseAddress = new Uri(_configuration.ApiBaseUri);
        }

        public async Task<GetExchangeRatesResponse> GetDailyExchangeRates(DateTime? date, string? lang)
        {
            var httpResponseMessage = await _client.GetAsync("exrates/daily"); // todo > outsource those URLs

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            // TODO: map response to target
            var asd = new GetExchangeRatesResponse();

            return asd;
        }
    }

    public class GetExchangeRatesResponse
    {
        public IEnumerable<ExchangeRateResponse> ExchangeRates { get; set; }
    }


    /*
     * To represent the objects received from the CNB Api
     *  
      "validFor": "2019-05-17",
      "order": 94,
      "country": "Australia",
      "currency": "dollar",
      "amount": 1,
      "currencyCode": "AUD",
      "rate": 15.858
     */
    public class ExchangeRateResponse
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
