using ExchangeRateUpdated.Service.Exceptions;

namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            ArgumentNullException.ThrowIfNull(code);
            InvalidCurrencyException.ThrowIfInvalid(code);

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

        public static implicit operator Currency(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            InvalidCurrencyException.ThrowIfInvalid(s);

            return new Currency(s);
        }

        public override bool Equals(object? obj)
        {
            var currency = obj as Currency;
            if (currency != null)
            {
                return currency.Code == Code;
            }

            var currencyCode = obj as string;
            if (currencyCode != null)
            {
                return currencyCode == Code;
            }

            return false;
        }
    }
}
