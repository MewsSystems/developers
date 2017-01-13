using ExchangeRateUpdater.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.NorwegianBank
{
    public class NorwegianBankExchangeRateProvider : IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string jsonString = null;
            //TODO: move to config
            string uri = "http://www.norges-bank.no/api/Currencies";

            //TODO: add error handling, move to separate method
            using (WebClient client = new WebClient())
            {
                //frequency=d2&language=en&idfilter=IDR,USD&observationlimit=2&returnsdmx=false
                client.QueryString.Add("frequency", "d2");
                client.QueryString.Add("language", "en");
                client.QueryString.Add("observationlimit", "2");
                client.QueryString.Add("returnsdmx", "false");
                client.QueryString.Add("idfilter", string.Join(",", currencies.Select(x => x.Code)));
                jsonString = client.DownloadString(uri);
            }

            //var response = JsonConvert.DeserializeObject<IEnumerable<NorwegianBankExchangeResponse>>(jsonString);
            var response = JsonConvert.DeserializeObject<NorwegianBankExchangeDetailedResponse>(jsonString);

            return response.TableEntries.Select(x => x.ToExchangeRate());
        }
    }
}
