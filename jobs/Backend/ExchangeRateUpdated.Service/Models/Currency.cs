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

        public static implicit operator Currency(string s)
        {
            if (s.Length != 3)
                throw new ArgumentException("Currency code can't be less than 3 characters");

            return new Currency(s);
        }
    }
}
