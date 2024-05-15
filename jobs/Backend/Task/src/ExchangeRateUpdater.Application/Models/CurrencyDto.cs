using Ardalis.GuardClauses;
using ExchangeRateUpdater.Domain.DomainExceptions;
using ExchangeRateUpdater.Domain.Entities;

namespace ExchangeRateUpdater.Application.Models
{
    public class CurrencyDto
    {
        public static implicit operator CurrencyDto(string code) => new CurrencyDto(code);

        public CurrencyDto(string code)
        {
            if( string.IsNullOrWhiteSpace(code) || code.Length != 3)
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
    }
}
