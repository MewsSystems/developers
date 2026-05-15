using System;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        public override bool Equals(object obj)
        {
            return obj is Currency currency && currency.Code.Equals(Code, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Code);
        }
    }
}
