using System.Diagnostics.CodeAnalysis;
using ExchangeRateUpdater.Domain.Models.Enums;

namespace ExchangeRateUpdater.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class Currency
    {
        public Currency(CurrencyCode code)
        {
            Code = code;
        }

        public Currency(string code)
        {
            Code = Enum.Parse<CurrencyCode>(code);
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public CurrencyCode Code { get; }

        public override string ToString()
        {
            return Code.ToString();
        }
    }
}
