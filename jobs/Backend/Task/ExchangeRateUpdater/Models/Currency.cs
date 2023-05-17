namespace ExchangeRateUpdater.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public Currency(string code, string country)
        {
            Code = code;
            Country = country;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The name of the country displayed in the language returned by the API.
        /// </summary>
        public string Country { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
