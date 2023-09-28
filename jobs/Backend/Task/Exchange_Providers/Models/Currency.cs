namespace ExchangeRateUpdater.Exchange_Providers.Models
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

        /// <summary>
        /// Returns the currency code as a string representation of the currency object.
        /// </summary>
        public override string ToString()
        {
            return Code;
        }
    }
}
