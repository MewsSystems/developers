namespace ExchangeRateUpdater.Models
{
    public class Currency
    {
        public Currency(string code = "CZK")
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
