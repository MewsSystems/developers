namespace ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates
{
    public class GetExchangeRatesQueryResponse
    {
        public required string SourceCurrency { get; set; }

        public required string TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public DateTime ValidFor {  get; set; }
    }
}
