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
        private const string _czechNationalBankApi = @"https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";
        private readonly HttpClient _httpClient;

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

            return ratesFromAPI.rates.ToList();
        }
    }
}
