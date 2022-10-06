using Mews.CurrencyExchange.Domain.Exceptions;

namespace Mews.CurrencyExchange.Domain.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            if (string.IsNullOrEmpty(code) )
                throw new InvalidCurrencyException();

            if (code.Length != 3)
                throw new InvalidCurrencyException("Currency is not a valid three-letter ISO 4217 code of the currency");

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
