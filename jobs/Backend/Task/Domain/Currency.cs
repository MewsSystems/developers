namespace ExchangeRateUpdater.Domain
{
    using System.Globalization;

    public class Currency
    {
        private readonly NumberFormatInfo _numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "." };

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
    }
}
