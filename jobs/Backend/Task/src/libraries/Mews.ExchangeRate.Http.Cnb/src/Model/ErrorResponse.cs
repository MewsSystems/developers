using System.Text.Json.Serialization;

namespace Mews.ExchangeRate.Http.Cnb.Model
{
    public class ErrorResponse
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("happenedAt")]
        public string HappenedAt { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
    }
}
