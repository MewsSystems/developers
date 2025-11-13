using Microsoft.AspNetCore.Http.Extensions;

namespace ExchangeRateService.Infrastructure;

public partial class LoggingEndpointFilter : IEndpointFilter
{
    [LoggerMessage(
        EventId = 13,
        Level = LogLevel.Information,
        Message = "{Method} {ApiUrl} started, {TraceIdentifier}.")]
    static partial void LogEndpointStarted(ILogger logger, string method, string apiUrl, string traceIdentifier);
    
    [LoggerMessage(
        EventId = 14,
        Level = LogLevel.Information,
        Message = "{Method} {ApiUrl} ended with {Result}, {TraceIdentifier}.")]
    static partial void LogEndpointEnded(ILogger logger, string method, string apiUrl, int result, string traceIdentifier);
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<LoggingEndpointFilter>>()!;
        var request = context.HttpContext.Request;
        var response = context.HttpContext.Response;
        var identifier = context.HttpContext.TraceIdentifier;
        
        LogEndpointStarted(logger, request.Method, request.GetDisplayUrl(), identifier);
        var result = await next(context);
        LogEndpointEnded(logger, request.Method, request.GetDisplayUrl(), response.StatusCode, identifier);

        return result;
    }
}