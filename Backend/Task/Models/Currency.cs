using System;

namespace ExchangeRateUpdater.Models
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
        public string Code { get; }

		public bool Equals(Currency other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return string.Equals(Code, other.Code);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((Currency)obj);
		}

		public override int GetHashCode()
		{
			return (Code != null ? Code.GetHashCode() : 0);
		}

		public override string ToString()
        {
            return Code;
        }
    }
}
