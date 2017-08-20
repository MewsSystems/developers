using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.RatesSource
{
    internal class ApiFixerSource : IExchangeRatesSource
    {
        RestClient client = new RestClient("http://api.fixer.io");

        public async Task<IEnumerable<ExchangeRate>> GetLatestRatesAsync(Currency baseCurrency, IEnumerable<Currency> requestedCurrencies)
        {
            var request = new RestRequest("latest");
            request.AddQueryParameter("base", baseCurrency.Code);
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
