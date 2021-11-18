using System;
using Core.Entities;

namespace ExchangeRateUpdater.Dtos
{
    public class ExchangeRateDto
    {
        public ExchangeRateDto(Currency sourceCurrency, Currency targetCurrency, decimal value)
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
            return $"{SourceCurrency}/{TargetCurrency}={String.Format("{0:n0}", Value)}";
        }
    }
}
