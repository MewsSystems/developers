using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace ExchangeRateUpdater.RatesSource
{
    internal class OpenExchangeSource :  IExchangeRatesSource
    {
        private const string appId = "e5d7b83fdece4a19b4cc4e24d97f82f2";
        RestClient client = new RestClient("https://openexchangerates.org/api");

        public async Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(Currency baseCurrency, IEnumerable<Currency> requestedCurrencies)
        {
            var request = new RestRequest("latest.json");
            request.AddQueryParameter("app_id", appId);
            request.AddQueryParameter("base", baseCurrency.Code); // To use different bases it's necessary to have a payed subscription
            string codes = string.Join(",", requestedCurrencies.Select(c => c.Code));
            request.AddQueryParameter("symbols", codes);

            var response = await client.ExecuteTaskAsync<RatesResult>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ReportFailure(response);
                return new List<ExchangeRate>();
            }

            return response.Data.Rates.Select(r => new ExchangeRate(new Currency(response.Data.Base), new Currency(r.Key), r.Value));
        }

        protected void ReportFailure(IRestResponse response)
        {
            Trace.WriteLine($"Unable to get exchange rates for url: {response.ResponseUri}");
        }

        internal class RatesResult
        {
            public string Base { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}