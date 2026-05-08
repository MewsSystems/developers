using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BadGatewayException : Exception
    {
        private const string DefaultMessage = "One or more bad gateway failures have occurred.";

        public BadGatewayException()
            : base(DefaultMessage)
        {
        }

        public BadGatewayException(string message)
            : base(message)
        {
        }

        public BadGatewayException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
