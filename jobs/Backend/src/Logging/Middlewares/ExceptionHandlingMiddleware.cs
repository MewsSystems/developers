using System.ComponentModel.DataAnnotations;
using Logging.Exceptions;
using Logging.Utils;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Logging.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = LoggingHttpContextHelper.GetCorrelationId(context);
        LoggingHttpContextHelper.SetCorrelationId(correlationId, context);

        try
        {
            await _next.Invoke(context);
        }
        catch (ValidationException e)
        {
            Log.Warning(e, e.Message);
            throw LoggingHelper.CreateBaseExceptionModel(e.Message, context.Response.StatusCode, correlationId);
        }
        catch (EmptyResultSetException e)
        {
            Log.Fatal(e, e.Message);
            throw LoggingHelper.CreateBaseExceptionModel($"Didn't fetch data... Correlation ID: {correlationId}", context.Response.StatusCode, correlationId);
        }
        catch (HttpRequestException e)
        {
            Log.Fatal(e, e.Message);
            throw LoggingHelper.CreateBaseExceptionModel(e.Message, context.Response.StatusCode, correlationId);
        }
        catch (XmlParsingException e)
        {
            Log.Fatal(e, e.Message);
            throw LoggingHelper.CreateBaseExceptionModel(e.Message, context.Response.StatusCode, correlationId);
        }
        catch (Exception e)
        {
            Log.Fatal(e, e.Message);
            throw LoggingHelper.CreateBaseExceptionModel($"Something went wrong... Correlation ID: {correlationId}", context.Response.StatusCode, correlationId);
        }
    }
}
