namespace ExchangeRateUpdater.Domain.Entities
{
    public class ExchangeRate
    {
        public ExchangeRate(CurrencyCode sourceCurrencyCode, CurrencyCode targetCurrencyCode, decimal value)
        {
            SourceCurrencyCode = sourceCurrencyCode;
            TargetCurrencyCode = targetCurrencyCode;
            Value = value;
        }

        public CurrencyCode SourceCurrencyCode { get; }

        public CurrencyCode TargetCurrencyCode { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrencyCode}/{TargetCurrencyCode}={Value}";
        }
    }
}
