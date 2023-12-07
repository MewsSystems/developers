
using Serilog.Context;
using Serilog.Core.Enrichers;
using Serilog;

namespace ExchangeRateUpdater.Host.WebApi.Middleware
{
    /// <summary>
    /// This middleware adds Requst Path and Query String to logging context.
    /// </summary>
    public class RequestMiddleware : IMiddleware
    {
        private Serilog.ILogger _logger;
        public RequestMiddleware(Serilog.ILogger? logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var path = new PropertyEnricher("RequestPath", context.Request.Path);
            var query = new PropertyEnricher("QueryString", context.Request.QueryString);

            using (LogContext.Push(new PropertyEnricher[] { path, query }))
            {
                _logger.Information("Request made to {RequestPath}?{QueryString}", context.Request.Path, context.Request.QueryString);
                await next.Invoke(context);
            }
        }
    }
}
