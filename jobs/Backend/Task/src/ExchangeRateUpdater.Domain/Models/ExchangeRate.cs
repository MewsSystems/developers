namespace ExchangeRateUpdater.Domain.Models
{
    public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal ExchangeValue, string ProviderName)
    {

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={ExchangeValue}";
        }
    }
}
