namespace ExchangeRateUpdater.Host.WebApi.Dtos.Request
{
    public class ExchangeOrderDto
    {
        public string? SourceCurrency { get; set; }
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
