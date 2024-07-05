using System;

namespace ExchangeRateUpdater.Models
{
    public class ApiErrorResponse
    {
        public string Description { get; set; }
        public string EndPoint { get; set; }
        public string ErrorCode { get; set; }
        public DateTime HappenedAt { get; set; }
        public string MessageId { get; set; }
    }
}
