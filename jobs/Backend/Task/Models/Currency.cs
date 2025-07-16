namespace ExchangeRateUpdater.Models
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
        public string Code { get; } // TODO: optionally validate ISO currency code format (length = 3, uppercase).

        public override string ToString()
        {
            return Code;
        }
    }
}
