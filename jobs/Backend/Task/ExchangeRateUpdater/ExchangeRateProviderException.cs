using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderException : Exception
    {
        public ExchangeRateProviderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
