namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code?.Trim();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        public bool Equals(Currency otherCurrency)
        {
            if(otherCurrency == null)
            {
                return false;
            }
            return string.Equals(Code, otherCurrency.Code, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
