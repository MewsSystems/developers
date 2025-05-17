namespace ExchangeRateUpdater
{
    public class ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
    {
        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
