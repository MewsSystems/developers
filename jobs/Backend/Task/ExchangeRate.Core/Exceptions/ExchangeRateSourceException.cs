using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ExchangeRate.Core.Exceptions;

[Serializable]
public class ExchangeRateSourceException : Exception
{
    public ExchangeRateSourceException() : base()
    {
    }

    public ExchangeRateSourceException(string message) : base(message)
    {
    }

    public ExchangeRateSourceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Constructor should be protected for unsealed classes, private for sealed classes.
    // (The Serializer invokes this constructor through reflection, so it can be private)
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected ExchangeRateSourceException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
        {
            throw new ArgumentNullException("info");
        }

        // MUST call through to the base class to let it save its own state
        base.GetObjectData(info, context);
    }
}
