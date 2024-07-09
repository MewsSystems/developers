using System;

namespace ExchangeRateUpdater.Lib.Shared
{
    [Serializable]
    public class EmptyContentException : Exception
    {
        public EmptyContentException() { }
        public EmptyContentException(string message) : base(message) { }
        public EmptyContentException(string message, Exception inner) : base(message, inner) { }
        protected EmptyContentException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
