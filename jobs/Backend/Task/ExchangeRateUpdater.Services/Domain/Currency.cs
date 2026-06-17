namespace ExchangeRateUpdater.Services.Domain
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

        public override string ToString()
        {
            return Code;
        }

        public static implicit operator Currency(string code)
        {
            return new Currency(code);
        }
    }
}
