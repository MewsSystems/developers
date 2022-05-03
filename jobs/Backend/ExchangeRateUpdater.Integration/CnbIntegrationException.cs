using System.Net;

namespace ExchangeRateUpdater.Integration
{
    /// <summary>
    /// Exception thrown when the integration api call fails.
    /// </summary>
    public class CnbIntegrationException : Exception
    {
        /// <summary>
        /// Status code returned by the client
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        public CnbIntegrationException(string? message, System.Net.HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
