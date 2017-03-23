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
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(Currency) && Equals((Currency) obj);
        }

        protected bool Equals(Currency other) => string.Equals(Code, other.Code, StringComparison.InvariantCulture);

        public override int GetHashCode() => StringComparer.InvariantCulture.GetHashCode(Code);

        public static bool operator ==(Currency left, Currency right) => Equals(left, right);

        public static bool operator !=(Currency left, Currency right) => !Equals(left, right);
    }
}
