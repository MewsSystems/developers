
using ExchangeRateUpdater.Domain.Exceptions;

namespace ExchangeRateUpdater.Domain.ValueObjects
{
    public class Currency : ValueObject
    {
        public const int MaxLenght = 3;
        public const string IsoCodeRequiredMsg = "Currency ISO code is required";
        public const string InvalidIsoCodeMsg = "Currency ISO code has 3 characters max lenght";

        public Currency(string code)
        {
            // Enforcing domain restrictions
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new DomainException(IsoCodeRequiredMsg);
            }

            if(code.Length > MaxLenght)
            {
                throw new DomainException(InvalidIsoCodeMsg);
            }

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

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }
    }
}
