namespace ExchangeRateProvider.Models
{
    public class CurrencyModel
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public CurrencyModel(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}