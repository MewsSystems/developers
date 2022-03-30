using Microsoft.AspNetCore.Http;

namespace Logging.Utils;

public static class LoggingHttpContextHelper
{
    public static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(LoggingConstants.CorrelationId, out var correlationIds);
        return correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();
    }

    public static void SetCorrelationId(string correlationId, HttpContext context) => context.Response.Headers.TryAdd($"x-{LoggingConstants.CorrelationId}", correlationId);
}
