namespace ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Dto
{
    /// <summary>
    /// Represents the response containing daily exchange rates retrieved from a REST API endpoint.
    /// </summary>
    internal class ExRateDailyResponse
    {
        /// <summary>
        /// Collection of daily exchange rates.
        /// </summary>
        public IEnumerable<ExDateDailyRest> Rates { get; set; } = [];
    }
}
