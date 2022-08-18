using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateApiServiceClient
{
    public class GetExchangeRatesResponse
    {
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}