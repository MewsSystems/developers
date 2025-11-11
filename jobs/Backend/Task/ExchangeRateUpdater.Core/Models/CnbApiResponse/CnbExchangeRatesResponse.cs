using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Core.Models.CnbApiResponse
{
    /// <summary>
    /// Class to describe the response from the CNB API.
    /// </summary>
    public class CnbExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public List<CnbExchangeRate> Rates { get; set; } = new List<CnbExchangeRate>();
    }
}