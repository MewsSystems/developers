using System.Runtime.Serialization;

namespace Mews.ExchangeRate.Domain.Exceptions;

[Serializable]
public class ExchangeRateException : Exception
{
    public ExchangeRateException(string message)
        : base(message) { }

    protected ExchangeRateException(SerializationInfo info,
        StreamingContext context)
        : base(info, context) { }
}
