using Refit;

namespace ExchangeRateUpdater
{
    public class GetExchangeRateRequest
    {
        [AliasAs("q")]
        public string Query { get; set; }

        [AliasAs("compact")]
        public string Compact => "y";
    }
}