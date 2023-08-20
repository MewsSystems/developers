namespace ExchangeRateUpdater.Domain
{
    public class Currency
    {
        /// <summary>
        /// In a real world scenario default currency should not be defined in code
        /// so that it can be different currency for different instances of the same service.
        /// This could be defined at appsettings level
        /// </summary>
        public static Currency DEFAULT_CURRENCY => new Currency("CZK");

        private Currency(string code)
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
