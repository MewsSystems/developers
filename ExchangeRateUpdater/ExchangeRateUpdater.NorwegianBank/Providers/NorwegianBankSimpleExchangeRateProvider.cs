using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.NorwegianBank.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.NorwegianBank.Providers
{
    [Obsolete("It shows rates for different converison factors which are not included in the response. Filtering by currencies is not implemented. \n Use NorwegianBankExchangeRateProvider instead", true)]
    public class NorwegianBankSimpleExchangeRateProvider : ExchangeRateProvider
    {
        public override string Url => "http://www.norges-bank.no/api/Currencies";

       //TODO: Implement filtering
        protected override IEnumerable<ExchangeRate> Convert(string response)
        {
            var parsedResponse = JsonConvert.DeserializeObject<NorwegianBankExchangeResponse[]>(response);
            var result = parsedResponse.Select(x => x.ExchangeRate);

            return result;
        }
    }
}
