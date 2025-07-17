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
        
        public override bool Equals(object? obj) => Equals(obj as Currency);

        public override int GetHashCode() => Code.GetHashCode(StringComparison.OrdinalIgnoreCase);

        private bool Equals(Currency? other) => other != null && Code == other.Code;
    }
}
