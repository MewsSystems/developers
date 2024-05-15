using Ardalis.GuardClauses;
using ExchangeRateUpdater.Domain.DomainExceptions;
using System;

namespace ExchangeRateUpdater.Domain.Entities
{
    public class Currency : IEquatable<Currency>
    {
        public static Currency Create(string currencyCode) => new Currency(currencyCode);
        private Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            {
                throw new InvalidCurrencyFormatException(code);
            }

            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; private set; }

        public override string ToString()
        {
            return Code;
        }

        public bool Equals(Currency other)
        {
            return other.Code.Equals(Code, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
