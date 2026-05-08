namespace ExchangeRateUpdater.Api.Models
{
    /// <summary>
    /// DTO for exchange rate information.
    /// </summary>
    public class ExchangeRateViewModel
    {
        /// <summary>
        /// Source currency code (e.g., USD, EUR).
        /// </summary>
        public string SourceCurrencyCode { get; set; } = string.Empty;

        /// <summary>
        /// Target currency code (typically CZK).
        /// </summary>
        public string TargetCurrencyCode { get; set; } = string.Empty;

        /// <summary>
        /// Exchange rate (how many target currency units per one source currency unit).
        /// </summary>
        public decimal Rate { get; set; }
    }
}