using ExchangeRateUpdater.Domain.Base;

namespace ExchangeRateUpdater.Domain
{
    public class Currency : ValueObject
    {
        private Currency(string code)
        {
            Code = code;
        }

        public static Currency Create(string code)
        {
            return new Currency(code);
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
