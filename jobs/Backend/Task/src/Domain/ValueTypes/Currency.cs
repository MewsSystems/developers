using System.Globalization;

namespace Domain.ValueTypes
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

        public bool IsValidCurrencyCode()
        {
            // Check for ISO 4217
            if (Code.Length == 3)
            {
                return true;
            }

            return false;
        }
    }
}
