namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string country, string currencyName, string code)
        {
            Country = country;
            CurrencyName = currencyName;
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Country { get; set; }
        public string CurrencyName { get; set; }
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
