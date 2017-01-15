using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.NorwegianBank.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.NorwegianBank.Providers
{
    /// <remarks>
    /// There are rates only for exchanging to NOK on the http://www.norges-bank.no/en/Statistics/exchange_rates/ site.
    /// Parameter currencies should contain "NOK" to retrieve a not-empty result.
    /// </remarks>
    public class NorwegianBankExchangeRateProvider : ExchangeRateProvider
    {
        public override string Url =>
            "http://www.norges-bank.no/api/Currencies?frequency=d2&language=en&observationlimit=1&returnsdmx=false";

        protected override string GetStringResponse(IEnumerable<string> currencyCodes)
        {
            string jsonString = null;

            if (ContainsNOK(currencyCodes))
            {
                using (WebClient client = new WebClient())
                {
                    string url = string.Concat(Url, "&idfilter=", string.Join(",", currencyCodes));
                    //client.QueryString.Add("idfilter", string.Join(",", currencyCodes));
                    jsonString = client.DownloadString(url);
                }
            }

            return jsonString;
        }

        protected override IEnumerable<ExchangeRate> Convert(string response, IEnumerable<string> currencyCodes)
        {
            if (!ContainsNOK(currencyCodes))
                return Enumerable.Empty<ExchangeRate>();

            if (string.IsNullOrWhiteSpace(response))
                throw new ArgumentNullException("response");

            var parsedResponse = JsonConvert.DeserializeObject<NorwegianBankExchangeDetailedResponse>(response);
            var result = parsedResponse.TableEntries.Select(x => x.ExchangeRate);
            return result;
        }

        private bool ContainsNOK(IEnumerable<string> currencyCodes) => currencyCodes.Any(code => code == "NOK");
    }
}
