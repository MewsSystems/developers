namespace ExchangeRateUpdater.Host.WebApi.Dtos.Request
{
    /// <summary>
    /// The Dto representing exchange order.
    /// </summary>
    public class ExchangeOrderDto
    {
        /// <summary>
        /// The code of the currency c
        /// </summary>
        public string? SourceCurrency { get; set; }
        /// <summary>
        /// The code of the currency which the exchange will happen to.
        /// </summary>
        public string? TargetCurrency { get; set; }
        /// <summary>
        /// The money that need to be exchanged.
        /// </summary>
        /// <remarks>
        /// Not sure if it is supposed to be decimal, but found that this website
        /// https://www.curs.md/en/convertor supports it.
        /// </remarks>
        public decimal? SumToExchange { get; set; }
    }
}
