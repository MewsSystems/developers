namespace ExchangeRateUpdater.Models
{
    public class Currency
    {
        public Currency(IsoCurrencyCode code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public IsoCurrencyCode Code { get; }

        public override string ToString()
        {
            return Code.ToString();
        }
    }
}
