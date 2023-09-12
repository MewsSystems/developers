using ExchangeRateUpdater.Domain.Models.Enums;

namespace ExchangeRateUpdater.Domain.Models.Dto
{
    public class ExchangeRateDto
    {
        public ExchangeRateDto(CurrencyCode sourceCurrency, CurrencyCode targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public CurrencyCode SourceCurrency { get; }

        public CurrencyCode TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
