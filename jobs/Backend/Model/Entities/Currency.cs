using System.Diagnostics.CodeAnalysis;

namespace Model.Entities
{
    public class Currency :  IEqualityComparer<Currency>
    {
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override int GetHashCode()
            => GetHashCode(this);

        public override bool Equals(object other)
            => other is Currency otherCurrency && Equals(otherCurrency);

        public bool Equals(Currency? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(this.Code.ToLowerInvariant(), other.Code.ToLowerInvariant());
        }

        public override string ToString()
            => Code;

        public bool Equals(Currency? x, Currency? y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode([DisallowNull] Currency obj)
            => obj.Code.GetHashCode();
    }
}
