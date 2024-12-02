using Czech_National_Bank_ExchangeRates.Infrastructure;
using Czech_National_Bank_ExchangeRates.Models;

namespace Czech_National_Bank_ExchangeRates.Repository
{
    public class CNBExchangeRateRepo : ICNBExchangeRateRepo
    {
        private readonly ICNBExchangeRateConnection _cnbExchangeRateConnection;
        private readonly IHttpClientService _httpClientService;
        public CNBExchangeRateRepo(ICNBExchangeRateConnection cnbExchangeRateConnection, IHttpClientService httpClientService)
        {
            _cnbExchangeRateConnection = cnbExchangeRateConnection;
            _httpClientService = httpClientService;
        }

        public async Task<ExchangeRates> GetExhangeRateData(string dateString)
        {
            string uri = _cnbExchangeRateConnection.Url
                .Replace("{date}", dateString);

            var res = await _httpClientService.GetAsync<ExchangeRates>
                                     (uri, string.Empty, string.Empty);
            return res;
        }
    }
}
