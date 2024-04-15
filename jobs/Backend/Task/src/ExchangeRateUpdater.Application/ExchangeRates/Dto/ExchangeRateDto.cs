namespace ExchangeRateUpdater.Application.ExchangeRates.Dto
{
    /// <summary>
    /// Data transfer object (DTO) representing an exchange rate between two currencies.
    /// </summary>
    public class ExchangeRateDto
    {
        /// <summary>
        /// Source currency of the exchange rate.
        /// </summary>
        public required CurrencyDto SourceCurrency { get; set; }

        /// <summary>
        /// Target currency of the exchange rate.
        /// </summary>
        public required CurrencyDto TargetCurrency { get; set; }

        /// <summary>
        /// Value of the exchange rate from 1 unit of the source currency to the target currency.
        /// </summary>
        public decimal Value { get; set; }
    }
}
