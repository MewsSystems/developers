using Swashbuckle.AspNetCore.Annotations;

namespace ExchangeRateUpdater.Api.Dtos
{
    /// <summary>
    /// Represents a request for exchange rate data.
    /// </summary>
    public class ExchangeRateRequestDto
    {
        /// <summary>
        /// Date in ISO format (yyyy-MM-dd). If omitted or invalid, defaults to today.
        /// </summary>
        [SwaggerSchema("Date in ISO format (yyyy-MM-dd). Default: Today's date if omitted or invalid.")]
        public string? Date { get; set; }

        /// <summary>
        /// Comma-separated list of currency codes (e.g. "USD,EUR,CZK").
        /// </summary>
        [SwaggerSchema("Comma-separated list of currency codes, e.g. 'USD,EUR,CZK'.")]
        public string Codes { get; set; } = "USD,EUR,CZK";
    }
}
