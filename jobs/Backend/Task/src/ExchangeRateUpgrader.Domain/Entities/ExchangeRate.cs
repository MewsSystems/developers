namespace ExchangeRateFinder.Domain.Entities
{
    public class CalculatedExchangeRate
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Value { get; set; }
    }
}
