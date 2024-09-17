namespace ExchangeRateUpdater.Common
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

        public bool Equals(Currency? other)
        {
            if (other is null)
            {
                return false;
            }
            return Code.Equals(other.Code);
        }

        public override string ToString()
        {
            return Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
