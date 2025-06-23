using System.Net;

namespace ExchangeRateUpdater.Domain.Ack
{
    public abstract class Ack
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }
}
