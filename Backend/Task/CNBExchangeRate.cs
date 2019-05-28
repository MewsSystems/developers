using System;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRate : ExchangeRate
    {
        public CNBExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, int amount)
            : base(sourceCurrency, targetCurrency, value)
        {
            this.Amount = amount;
        }

        public int Amount { get; }

        public new decimal Value => base.Value * this.Amount;
    }
}
