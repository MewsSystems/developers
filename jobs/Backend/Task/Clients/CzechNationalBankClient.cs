using ExchangeRateUpdater.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Clients
{
    public interface ICzechNationalBankClient
    {
        Task<HttpResponseMessage> GetExchangeRates();
    } 

    public class CzechNationalBankClient : BaseApiClient, ICzechNationalBankClient
    {

        private readonly string _clientUrl;
        private static HttpClient _httpClient;
        private static readonly object LockObject = new();

        public CzechNationalBankClient(ICzechNationalBankConfiguration configuration)
        {
            _clientUrl = configuration.ExchangeRateUri();
        }

        public override HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                lock (LockObject)
                {
                    if (_httpClient == null)
                    {
                        _httpClient = new HttpClient();
                    }
                }
            }
            return _httpClient;
        }

        public async Task<HttpResponseMessage> GetExchangeRates()
        {
            var url = new Uri($"{_clientUrl}");
            return await Execute<HttpResponseMessage>(HttpMethod.Get, url, null);
        }
    }
}