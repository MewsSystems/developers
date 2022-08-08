using System;

namespace ExchangeRateUpdater
{
    public class SourceUnavailableException : Exception
    {
        public SourceUnavailableException()
        {
        }

        public SourceUnavailableException(string message)
            : base(message)
        {
        }

        public SourceUnavailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}