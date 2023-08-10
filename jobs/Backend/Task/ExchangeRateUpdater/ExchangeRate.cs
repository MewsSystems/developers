namespace ExchangeRateUpdater
{
    public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
    {
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
 