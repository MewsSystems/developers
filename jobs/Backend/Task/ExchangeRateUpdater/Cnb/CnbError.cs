using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdater.Cnb;

public class CnbError;

public sealed class CnbUnexpectedStatusError(HttpStatusCode statusCode) : CnbError
{
    public HttpStatusCode StatusCode => statusCode;
}

public sealed class CnbInvalidPayloadError(JsonException exception, string payload) : CnbError
{
    public CnbInvalidPayloadError(string payload)
        : this(null!, payload)
    {
    }

    public JsonException? Exception { get; } = exception;
    public string RawPayload { get; } = payload;
}

public sealed class CnbTimeoutError : CnbError;