namespace ExchangeRateUpdater.Models
{
    public class Currency
    {
        public Currency(string country, string currencyName, string currencyCode)
        {
            Country = country;
            Name = currencyName;
            Code = currencyCode;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }
        public string Country { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
