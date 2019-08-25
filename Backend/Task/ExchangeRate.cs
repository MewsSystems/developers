using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, int units, double value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = Convert.ToDecimal(value / units); //Some currencies are traded in multiple units.  This will convert the publised rate as if traded in single units.
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }
        public decimal Value { get; }

        public override string ToString()
        {
            return $"{SourceCurrency.Code}/{TargetCurrency.Code}={Value}";
        }
    }
}
