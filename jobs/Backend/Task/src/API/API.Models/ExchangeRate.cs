namespace API.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            if (sourceCurrency is null)
            {
                throw new ArgumentNullException(nameof(sourceCurrency), "Source currency cannot be null.");
            }

            if (targetCurrency is null)
            {
                throw new ArgumentNullException(nameof(targetCurrency), "Target currency cannot be null.");
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be negative.");
            }

            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
