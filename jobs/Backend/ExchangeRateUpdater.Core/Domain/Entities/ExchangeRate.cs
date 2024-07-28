using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, decimal sourceValue, Currency targetCurrency, decimal targetValue)
        {
            SourceCurrency = sourceCurrency;
            SourceValue = sourceValue;
            TargetCurrency = targetCurrency;
            TargetValue = targetValue;
        }

        public Currency SourceCurrency { get;  }
        public decimal SourceValue { get;  }
        public Currency TargetCurrency { get; }
        public decimal TargetValue { get; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={TargetValue}";
        }
    }
}
