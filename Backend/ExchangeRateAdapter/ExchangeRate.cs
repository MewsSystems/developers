namespace ExchangeRateAdapter
{
    public class ExchangeRate
    {
        public ExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, decimal value)
        {
            SourceCurrencyCode = sourceCurrencyCode;
            TargetCurrencyCode = targetCurrencyCode;
            Value = value;
        }

        public string SourceCurrencyCode { get; }

        public string TargetCurrencyCode { get; }

        public decimal Value  { get; }

    }
}
