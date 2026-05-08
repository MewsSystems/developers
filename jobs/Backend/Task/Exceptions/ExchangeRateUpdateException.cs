namespace ExchangeRateUpdater.Exceptions
{
    using System;

    public class ExchangeRateUpdateException : Exception
    {
        public ExchangeRateUpdateException()
        {
        }

        public ExchangeRateUpdateException(string message) : base(message)
        {
        }

        public ExchangeRateUpdateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
