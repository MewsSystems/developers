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
            var currency = obj as Currency;
            return currency != null && Code.Equals(currency.Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
