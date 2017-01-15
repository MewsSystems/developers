using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.NorwegianBank.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.NorwegianBank.Providers
{
    public class NorwegianBankExchangeRateProvider : ExchangeRateProvider
    {
        public override string Url =>
            "http://www.norges-bank.no/api/Currencies?frequency=d2&language=en&observationlimit=1&returnsdmx=false";

        protected override string GetStringResponse(IEnumerable<string> currencyCodes)
        {
            string jsonString = null;

            //TODO: add error handling, move to separate method
            using (WebClient client = new WebClient())
            {
                string url = string.Concat(Url, "&idfilter=", string.Join(",", currencyCodes));
                //client.QueryString.Add("idfilter", string.Join(",", currencyCodes));
                jsonString = client.DownloadString(url);
            }

            return jsonString;
        }

        protected override IEnumerable<ExchangeRate> Convert(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                throw new ArgumentNullException("Response is null");

            var parsedResponse = JsonConvert.DeserializeObject<NorwegianBankExchangeDetailedResponse>(response);
            var result = parsedResponse.TableEntries.Select(x => x.ExchangeRate);

            return result;
        }
    }
}
