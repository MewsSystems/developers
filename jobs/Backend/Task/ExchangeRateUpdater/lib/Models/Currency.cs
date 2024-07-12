namespace ExchangeRateUpdater.Lib.Shared
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        public override int GetHashCode()
        {
            return Code != null ? Code.GetHashCode() : 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Currency otherCurrency)
            {
                return Code == otherCurrency.Code;
            }
            return false;
        }
    }
}
