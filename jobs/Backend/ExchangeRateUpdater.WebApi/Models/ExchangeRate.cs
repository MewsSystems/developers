namespace ExchangeRateUpdater.WebApi.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
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

        public override bool Equals(object? exchangeRate)
        {
            if (exchangeRate == null)
            {
                return false;
            }

            if (!(exchangeRate is ExchangeRate))
            {
                return false;
            }

            return (SourceCurrency.Code.Equals(((ExchangeRate)exchangeRate).SourceCurrency.Code) &&
                    TargetCurrency.Code.Equals(((ExchangeRate)exchangeRate).TargetCurrency.Code) &&
                    Value.Equals(((ExchangeRate)exchangeRate).Value));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceCurrency.Code, TargetCurrency.Code, Value.GetHashCode());
        }
    }
}
