using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb
{
    internal interface IBankApi
    {
        Task<IEnumerable<CnbExchangeRate>> GetTodayExchangeRates();
    }

    /// <summary>
    ///     Api for Česká národní banka
    ///     <seealso href="https://api.cnb.cz/cnbapi/swagger-ui.html"/> 
    /// </summary>
    internal sealed class CnbApi : IBankApi
    {
        private readonly HttpClient _httpClient;

        public CnbApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetTodayExchangeRates()
        {
            const string exchangeRatesEndpoint = "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";
            var response = await _httpClient.GetFromJsonAsync<CnbExchangeRates>(exchangeRatesEndpoint);
            if (response == null)
            {
                throw new InvalidOperationException("Unable to get exchange rates. Response was null.");
            }
            return response.Rates;
        }
    }
}
