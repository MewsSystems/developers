using System.Net;
using Logging.Exceptions;

namespace Logging.Utils;

public static class LoggingHelper
{
    public static BaseExceptionModel CreateBaseExceptionModel(string message, int statusCode, string correlationId) => new ()
    {
        Message = message,
        StatusCode = (HttpStatusCode)statusCode,
        CorrelationId = correlationId
    };
}
