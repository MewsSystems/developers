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
            return obj is Currency other && Equals(other);
        }

        private bool Equals(Currency other)
        {
            return Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code != null ? Code.GetHashCode() : 0;
        }
    }
}