namespace ExchangeRateUpdater.Domain.Models
{
    public record ExchangeRate
    {
        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal ExchangeValue { get; }
        public string? ProviderName { get; }
        public DateTime? ValidUntil { get; }

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal exchangeValue, string? providerName = null, DateTime? validUntil = null)
        {
            // Validation
            SourceCurrency = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency), "Source currency cannot be null");
            TargetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency), "Target currency cannot be null");
            
            if (exchangeValue <= 0)
            {
                throw new ArgumentException("Exchange value must be positive", nameof(exchangeValue));
            }
            
            ExchangeValue = exchangeValue;
            ProviderName = providerName;
            ValidUntil = validUntil;
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={ExchangeValue}";
        }
    }
}
