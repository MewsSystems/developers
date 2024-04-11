using MewsFinance.Domain.Models;

namespace MewsFinance.Application.UseCases.ExchangeRates.Queries
{
    public class ExchangeRateResponse
    {
        public ExchangeRateResponse(Currency sourceCurrency, Currency targetCurrency, decimal value)
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
    }
}
