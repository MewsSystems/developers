using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICnbApiWrapper
    {
        Task<List<ExchangeRateRecord>> GetExchangeRatesAsync(bool logDataToConsole = false);
    }

    public class CnbApiWrapper : ICnbApiWrapper
    {
        private readonly HttpClient _httpClient;

        // Max: Candidate to Configuration File
        private const string _czechNationalBankApi = @"https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

        // Max: Improve with IHttpClientFactory with Dependency injection
        // private readonly IHttpClientFactory _clientFactory;
        // _clientFactory = clientFactory;
        // using var httpClient = _clientFactory.CreateClient();

        public CnbApiWrapper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ExchangeRateRecord>> GetExchangeRatesAsync(bool logDataToConsole = false)
        {
            var ratesFromAPI = await _httpClient.GetFromJsonAsync<ExchangeRatesRecord>(_czechNationalBankApi);

            if (logDataToConsole)
            {
                Console.WriteLine("CNB Rates");
                Console.WriteLine($"validFor,order,country,currency,amount,currencyCode,rate");
                foreach (var rate in ratesFromAPI.rates)
                    Console.WriteLine($"{rate.validFor},{rate.order},{rate.country},{rate.currency},{rate.amount},{rate.currencyCode},{rate.rate}");
            }

            // Max: handle possible exceptions
            return ratesFromAPI.rates.ToList();
        }
    }
}
