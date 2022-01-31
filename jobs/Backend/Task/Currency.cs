namespace ExchangeRateUpdater
{
    /// <summary>
    /// This could all go to EF, currency perhaps could be an owned entity here.
    /// </summary>
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
    }
}
