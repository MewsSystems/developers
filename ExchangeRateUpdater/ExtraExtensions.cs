using System;

namespace ExchangeRateUpdater
{
    public static class ExtraExtensions
    {
        public static bool IsEqual(this Currency cur, Currency otherCurrency)
        {
            if (Currency.IsNullOrEmpty(cur) || Currency.IsNullOrEmpty(otherCurrency)) throw new Exception(Res.CurrenciesShouldBeSet);
            return (string.Compare(cur.Code, otherCurrency.Code, true) == 0);
        }
    }
}
