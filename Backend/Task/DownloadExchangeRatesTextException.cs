using System;
using System.Runtime.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable]
    internal class DownloadExchangeRatesTextException : Exception
    {
        public DownloadExchangeRatesTextException()
        {
        }

        public DownloadExchangeRatesTextException(string message) : base(message)
        {
        }

        public DownloadExchangeRatesTextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DownloadExchangeRatesTextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}