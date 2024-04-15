namespace ExchangeRateUpdater.Application.ExchangeRates.Dto
{
    /// <summary>
    /// Data transfer object (DTO) representing a currency.
    /// </summary>
    public class CurrencyDto
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public required string Code { get; set; }
    }
}
