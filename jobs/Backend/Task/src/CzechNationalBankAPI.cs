using ExchangeRateUpdaterModels.Models;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.src
{
    /// <summary>
    /// Class to interact with the Czech National Bank API to fetch exchange rates.
    /// </summary>
    public class CzechNationalBankAPI
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CzechNationalBankAPI"/> class.
        /// </summary>
        public CzechNationalBankAPI()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CzechNationalBankAPI"/> class with a specified HttpClient.
        /// </summary>
        /// <param name="httpClient">The HttpClient to be used for making requests.</param>
        public CzechNationalBankAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Asynchronously gets the exchange rates from the Czech National Bank API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ExchangeRateModel"/>.</returns>
        public async Task<IEnumerable<ExchangeRateModel>> GetRatesAsync()
        {
            List<ExchangeRateModel> exchangeRates = new List<ExchangeRateModel>();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://api.cnb.cz/cnbapi/exrates/daily");
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                foreach (var rate in json["rates"])
                {
                    exchangeRates.Add(new ExchangeRateModel(
                        new CurrencyModel(rate["currencyCode"].ToString()),
                        new CurrencyModel("CZK"), // Assuming target currency is CZK
                        decimal.Parse(rate["rate"].ToString())
                    ));
                }
                return exchangeRates;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error fetching exchange rates from CNB API");
                return exchangeRates;
            }
        }
    }
}
