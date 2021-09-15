namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, decimal amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
            Amount = amount;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public decimal Amount { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}; Base Amount={Amount}";
        }

        public bool Equals(ExchangeRate otherExchangeRate)
        {
            return ((SourceCurrency != null && otherExchangeRate.SourceCurrency != null) && SourceCurrency.Equals(otherExchangeRate.SourceCurrency))
                && ((TargetCurrency != null && otherExchangeRate.TargetCurrency != null) && TargetCurrency.Equals(otherExchangeRate.TargetCurrency))
                && Value == otherExchangeRate.Value
                && Amount == otherExchangeRate.Amount;
        }
    }
}
