namespace ExchangeRateUpdater.Domain.Models
{
    public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal ExchangeValue, string ProviderName, DateTime ValidUntil)
    {
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={ExchangeValue}";
        }
    }
}
