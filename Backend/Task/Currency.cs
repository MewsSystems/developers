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

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            var currency = obj as Currency;
            return (currency != null) && string.Equals(this.Code, currency.Code, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode() {
            return this.Code.GetHashCode();
        }
    }
}
