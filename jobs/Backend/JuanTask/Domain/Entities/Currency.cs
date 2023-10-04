using Domain.Exceptions;

namespace Domain.Entities
{
    public class Currency
    {

        private const int CurrencyCodeLength = 3;

        public Currency(string code)
        {

            if (string.IsNullOrEmpty(code) || code.Length != CurrencyCodeLength)
                throw new CurrencyInvalidLengthException(code);

            if (!code.All(char.IsLetter))
                throw new CurrencyMustBeAllLettersException(code);

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
