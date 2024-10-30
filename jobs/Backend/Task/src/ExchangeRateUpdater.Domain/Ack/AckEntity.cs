using System.Net;

namespace ExchangeRateUpdater.Domain.Ack
{
    public class AckEntity<T> : Ack
    {
        public AckEntity(bool success, string message = "", HttpStatusCode? code = null)
        {
            Success = success;
            Message = message;
            StatusCode = code;
        }

        public AckEntity(bool success, T entity, string message = "", HttpStatusCode? code = null)
        {
            Entity = entity;
            Success = success;
            Message = message;
            StatusCode = code;
        }

        public T? Entity { get; set; }
    }
}
