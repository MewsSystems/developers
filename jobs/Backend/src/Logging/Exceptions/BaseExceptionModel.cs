using System.Net;

namespace Logging.Exceptions;

public class BaseExceptionModel : Exception
{
    public new string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string CorrelationId { get; set; }
}
