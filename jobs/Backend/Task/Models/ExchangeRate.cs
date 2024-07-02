namespace ExchangeRateUpdater.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal rate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
        }

        public Currency SourceCurrency { get; } // Base

        public Currency TargetCurrency { get; } // Quote

        public decimal Rate { get; } // MidRate

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Rate}";
        }
    }
}
