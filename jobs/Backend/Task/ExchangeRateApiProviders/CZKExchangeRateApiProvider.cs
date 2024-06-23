using System;
using System.Collections.Generic;


namespace ExchangeRateUpdater
{
    public class ExchangeRateCNBResult
    {
        public DateTime validFor { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public decimal rate { get; set; }
    }

    public class ExchangeRateCNBApiResponse
    {
        public ExchangeRateCNBResult[] rates { get; set; }
    }

    public class CZKExchangeRateApiProvider : BaseExchangeRateApiProvider<ExchangeRateCNBApiResponse>
    {
        public override string ApiEndpoint => "https://api.cnb.cz/cnbapi/exrates/daily?lang=EN";

        protected override ExchangeRatesDictionary TransformToDictionary(ExchangeRateCNBApiResponse response)
        {
            var exchangeRates = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var rate in response.rates)
            {
                if (rate.amount == 0)
                {
                    continue;
                }

                exchangeRates[rate.currencyCode] = new Dictionary<string, decimal>
                {
                    { "CZK", rate.rate/rate.amount }
                };
            }

            return new ExchangeRatesDictionary(exchangeRates);
        }
    }
}
