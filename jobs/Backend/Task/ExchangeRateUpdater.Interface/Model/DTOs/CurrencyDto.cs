using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Interface.DTOs
{
    public class CurrencyDto
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        public override string ToString()
        {
            return Code;
        }
    }
}
