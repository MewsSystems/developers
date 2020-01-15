namespace ExchangeRateUpdater
{
    public class Currency
    {
        public static readonly Currency Czech = new Currency("CZK");

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
    }
}
