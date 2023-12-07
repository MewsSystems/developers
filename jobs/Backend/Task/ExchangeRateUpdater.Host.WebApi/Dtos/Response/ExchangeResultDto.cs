namespace ExchangeRateUpdater.Host.WebApi.Dtos.Response
{
    /// <summary>
    /// The Dto representing the exchange order result.
    /// </summary>
    public class ExchangeResultDto
    {
        /// <summary>
        /// The code of the currency which was exchanged.
        /// </summary>
        public string? SourceCurrency { get; set; }
        /// <summary>
        /// The code of the currency after the exchange happened.
        /// </summary>
        public string? TargetCurrency { get; set; }
        /// <summary>
        /// The money sum after the exchange happen in target currency.
        /// </summary>
        public decimal ConvertedSum { get; set; }
        /// <summary>
        /// The date of the exchange rate.
        /// </summary>
        public DateTime ExchangeRateDate { get; set; }
    }
}
