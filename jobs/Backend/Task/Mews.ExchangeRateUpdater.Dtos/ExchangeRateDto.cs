namespace Mews.ExchangeRateUpdater.Dtos
{
    /// <summary>
    /// This is the DTO which is returned as a response for the input CurrencyDto collection
    /// </summary>
    public class ExchangeRateDto
    {
        public ExchangeRateDto(CurrencyDto sourceCurrency, CurrencyDto targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public CurrencyDto SourceCurrency { get; }

        public CurrencyDto TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
