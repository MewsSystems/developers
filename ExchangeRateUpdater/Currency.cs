using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 3)
                throw new Exception(Res.ErrorCurrencyCode);
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; private set; }

        public static bool IsNullOrEmpty(Currency currency)
        {
            return (currency == null || string.IsNullOrEmpty(currency.Code));
        }
    }

}
