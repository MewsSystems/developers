using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Model
{
    public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value, DateOnly Date)
    {
        public bool IsFromTo(Currency sourceCurrency, Currency targetCurrency)
        {
            return SourceCurrency == sourceCurrency && TargetCurrency == targetCurrency;
        }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
