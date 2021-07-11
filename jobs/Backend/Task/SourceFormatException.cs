using System;
using System.Runtime.Serialization;

namespace ExchangeRateUpdater
{
    [Serializable]
    public sealed class SourceFormatException : Exception
    {
        public SourceFormatException()
        {
        }

        public SourceFormatException(string message)
            : base(message)
        {
        }

        public SourceFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private SourceFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
