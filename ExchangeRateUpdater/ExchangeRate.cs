using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            if (Currency.IsNullOrEmpty(sourceCurrency) || Currency.IsNullOrEmpty(targetCurrency))
                throw new Exception(Res.CurrenciesShouldBeSet);
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; private set; }

        public Currency TargetCurrency { get; private set; }

        public decimal Value { get; private set; }

        public override string ToString()
        {
            return SourceCurrency.Code + "/" + TargetCurrency.Code + "=" + Value;
        }
    }
}
