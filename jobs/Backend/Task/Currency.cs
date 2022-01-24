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

        public bool Equals(Currency curr)
        {
            if (curr == null)
                return false;
            return this.Code == curr.Code;
        }

        public override bool Equals(object obj)
        {
            if (obj is Currency)
                return Equals((Currency)obj);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
