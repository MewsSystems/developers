using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.DTOs
{
    public class ExchangeRateProviderDTO
    {
        public ExchangeRateProviderDTO(CurrencyDTO sourceCurrency, CurrencyDTO targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public CurrencyDTO SourceCurrency { get; }

        public CurrencyDTO TargetCurrency { get; }

        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
