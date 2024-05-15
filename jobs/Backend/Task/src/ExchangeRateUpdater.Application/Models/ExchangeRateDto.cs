using Ardalis.GuardClauses;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Models
{
    public class ExchangeRateDto
    {
        public ExchangeRateDto(CurrencyDto sourceCurrency, CurrencyDto targetCurrency, decimal value)
        {
            SourceCurrency = Guard.Against.Null(sourceCurrency);
            TargetCurrency = Guard.Against.Null(targetCurrency);
            Value = Guard.Against.NegativeOrZero(value);
        }

        public CurrencyDto SourceCurrency { get; private set; }

        public CurrencyDto TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
