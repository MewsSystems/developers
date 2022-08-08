using System;

namespace ExchangeRateUpdater
{
    public class NoParserAvailableException : Exception
    {
        public NoParserAvailableException()
        {
        }

        public NoParserAvailableException(string message)
            : base(message)
        {
        }

        public NoParserAvailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}