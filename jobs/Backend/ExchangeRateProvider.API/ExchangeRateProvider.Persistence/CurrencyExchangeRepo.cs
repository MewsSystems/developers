using ExchangeRateProvider.Persistence.IRepo;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace ExchangeRateProvider.Persistence
{
    public class CurrencyExchangeRepo : ICurrencyExchangeRepo
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;

        public CurrencyExchangeRepo(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            setBaseAddress();
        }

        public async Task<string> GetPairsAsync(string uri)
        {
            return await _httpClient.GetStringAsync(uri);
        }

        private void setBaseAddress()
        {
            _httpClient.BaseAddress = new 
                Uri(_configuration["CzechNationalBankApiProperties:BaseAddress"]);
        }
    }
}
