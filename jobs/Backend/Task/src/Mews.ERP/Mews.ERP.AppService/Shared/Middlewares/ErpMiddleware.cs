using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Mews.ERP.AppService.Shared.Middlewares;

[ExcludeFromCodeCoverage]
public class ErpMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ErpMiddleware> logger;

    public ErpMiddleware(RequestDelegate next, ILogger<ErpMiddleware> logger)
    {
        _next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        catch (Exception e)
        {
            // We can define and implement our global error handling strategy here.
            // For the time being and for the purpose of this test we only log the error.
            logger.LogError(e, "Error caught by global exception handler");
        }
    }
}