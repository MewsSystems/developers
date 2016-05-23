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
        public string Code { get; private set; }
    }
}
