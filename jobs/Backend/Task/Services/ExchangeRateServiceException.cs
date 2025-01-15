using System;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateServiceException : Exception
    {
        public ExchangeRateServiceException(string message) : base(message) { }
    }
}
