namespace ExchangeRateFinder.Domain.Entities
{
    public class CalculatedExchangeRate
    {
        public string Id => $"{SourceCurrencyCode}-{TargetCurrencyCode}";
        public string SourceCurrencyCode { get; init; } = string.Empty;

        public string TargetCurrencyCode { get; init; } = string.Empty;

        public decimal Rate { get; init; }
    }
}
