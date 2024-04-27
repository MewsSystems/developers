namespace ExchangeRateFinder.Domain.Entities
{
    public class CalculatedExchangeRate
    {
        public string Id => $"{SourceCurrencyCode}-{TargetCurrencyCode}";
        public string SourceCurrencyCode { get; set; }

        public string TargetCurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }
}
