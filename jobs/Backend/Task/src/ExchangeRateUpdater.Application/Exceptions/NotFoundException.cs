using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "One or more not found failures have occurred.";

        public NotFoundException()
            : base(DefaultMessage)
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
