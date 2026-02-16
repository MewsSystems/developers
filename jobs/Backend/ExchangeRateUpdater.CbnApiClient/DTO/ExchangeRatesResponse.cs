using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CbnApiClient.Dto
{
    /// <summary>
    /// Represents the response returned by the CBN API containing a collection of exchange rate entries.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExchangeRatesResponse
    {
        /// <summary>
        /// Collection of rate entries returned by the CBN API.
        /// </summary>
        [JsonPropertyName("rates")]
        public List<ExchangeRateEntry> Rates { get; set; } = [];
    }
}
