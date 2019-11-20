using System;

namespace ExchangeRateUpdater
{
    public class Currency : IEquatable<Currency>
    {
        public Currency(string code)
        {
            if (code == null)
                throw new ArgumentNullException(nameof(code));
            if (code.Length != 3)
                throw new ArgumentException($"'{code}' is not a currency ISO 4217 code", nameof(code));

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Currency other)
            {
                return Equals(other);
            }
            else
                return false;
        }

        public override int GetHashCode() => Code.GetHashCode();

        public static bool operator ==(Currency c1, Currency c2) => c1.Equals(c2);
        public static bool operator !=(Currency c1, Currency c2) => !c1.Equals(c2);

        public bool Equals(Currency other) => Code == other.Code;

        public override string ToString() => Code;
    }
}
