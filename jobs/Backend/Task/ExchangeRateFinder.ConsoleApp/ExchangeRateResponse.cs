namespace ExchangeRateFinder.Domain.Entities
{
    public class ExchangeRateResponse
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
