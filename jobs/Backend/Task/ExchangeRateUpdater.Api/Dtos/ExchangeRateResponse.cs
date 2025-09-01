namespace ExchangeRateUpdater.Api.Dtos
{
    public class ExchangeRateResponse
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Value { get; set; }
    }
}
