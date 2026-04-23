using System.Net;

namespace ExchangeRateUpdater.Domain.Model.Polly
{
    public class PollyResultModel<T>
    {
        public HttpStatusCode Code { get; set; }
        public T Response { get; set; }
    }
}
