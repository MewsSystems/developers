using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Entities
{
    /// <summary>
    /// Represents an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal rate, DateTime effectiveDate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
            EffectiveDate = effectiveDate;
        }

        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public decimal Rate { get; }
        public DateTime EffectiveDate { get; }
    }
}
