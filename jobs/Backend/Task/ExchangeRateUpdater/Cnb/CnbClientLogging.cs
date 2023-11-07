using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Cnb;

// 💡 this is a bit of stretch, but it may positively affect performance (avoid boxing);
//    good for generic libraries, but probably not worth in regular application
internal static partial class CnbClientLogging
{
    [LoggerMessage(EventId = 9001, Level = LogLevel.Error, Message = "Request to CNB API timed out")]
    public static partial void RequestTimedOut(this ILogger<CnbClient> logger, TaskCanceledException exception);

    [LoggerMessage(EventId = 9002, Level = LogLevel.Error, Message = "Received unexpected status code from CNB API: {StatusCode} {Payload}")]
    public static partial void UnexpectedStatusCode(this ILogger<CnbClient> logger, HttpStatusCode statusCode, string payload);

    [LoggerMessage(EventId = 9003, Level = LogLevel.Error, Message = "Failed to deserialize CNB API response: {Payload}")]
    public static partial void FailedToDeserializePayload(this ILogger<CnbClient> logger, JsonException exception, string payload);

    [LoggerMessage(EventId = 9004, Level = LogLevel.Error, Message = "Received invalid payload from CNB API: {Payload}")]
    public static partial void InvalidPayload(this ILogger<CnbClient> logger, string payload);
}