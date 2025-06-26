using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Cnb
{
    public class ApiErrorResponse
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("endPoint")]
        public string EndPoint { get; set; }
        
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }
        
        [JsonPropertyName("happenedAt")]
        public DateTime HappenedAt { get; set; }
        
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
    }
}
