using System;

namespace ExchangeRateUpdater
{
    public class Currency : IEquatable<Currency>
    {
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; private set; }

        public bool Equals(Currency other) => other.Code == Code;
    }
}
