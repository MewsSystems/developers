using System;
using System.Runtime.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable]
    public sealed class ExchangeRatesSourceException : Exception
    {
        public ExchangeRatesSourceException()
        {
        }

        public ExchangeRatesSourceException(string message)
            : base(message)
        {
        }

        public ExchangeRatesSourceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private ExchangeRatesSourceException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
