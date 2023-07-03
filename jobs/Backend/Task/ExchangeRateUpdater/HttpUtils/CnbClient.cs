using ExchangeRateUpdater.DTO;
using ExchangeRateUpdater.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.HttpUtils
{
    public class CnbClient : ICnbClient
    {
        private readonly HttpClient _httpClient;

        public CnbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ExchangeRate>> GetCurrentExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(new Uri("cnbapi/exrates/daily", UriKind.Relative));

            if(response.StatusCode != HttpStatusCode.OK) 
            {
                throw new ExchangeRatesException($"Failed to get the list of exchange rates. Status code: {response.StatusCode}", true);
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var rates = JObject.Parse(responseString)["rates"]?.ToString();

            if(rates == null) 
            {
                throw new ExchangeRatesException("Unrecognized content of the response", false);
            }

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };

            var exchangeRateDTOs = JsonConvert.DeserializeObject<IEnumerable<ExchangeRateDTO>>(rates, jsonSerializerSettings);
            
            var currencyCodes = currencies.Select(c => c.Code);
            var result = exchangeRateDTOs.Where(r => currencyCodes.Contains(r.CurrencyCode)).Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate));

            return result;
        }
    }
}
