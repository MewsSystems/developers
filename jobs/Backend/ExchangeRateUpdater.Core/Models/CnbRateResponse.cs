using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Models
{
    /// <summary>
    /// Represents the JSON response from the Czech National Bank (CNB) exchange rate API.
    /// </summary>
    public class CnbRateResponse
    {
        /// <summary>
        /// A list of exchange rate items retrieved from the CNB API.
        /// </summary>
        [JsonPropertyName("rates")]
        public required List<CnbRateItem> Rates { get; set; }

        /// <summary>
        /// The start date of the exchange rates in the response.
        /// </summary>
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the exchange rates in the response.
        /// </summary>
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The name of the exchange rate source, if provided by the API.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
