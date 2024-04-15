using Refit;

namespace ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi.Queries
{
    /// <summary>
    /// Represents a query for exchang rates language and date parameters.
    /// </summary>
    internal class ExchangeRatesQuery(Language language, string? date)
    {
        /// <summary>
        /// Language for localization.
        /// </summary>
        [AliasAs("lang")]
        public Language Language { get; set; } = language;

        /// <summary>
        /// Date for which exchange rates are requested.
        /// If null, the latest available rates are fetched.
        /// </summary>
        [AliasAs("date")]
        public string? Date { get; set; } = date;
    }
}
