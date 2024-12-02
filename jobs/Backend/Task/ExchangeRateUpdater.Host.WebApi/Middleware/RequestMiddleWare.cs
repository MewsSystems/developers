
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
        
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="logger">Instance of <see cref="Serilog.ILogger"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RequestMiddleware(Serilog.ILogger? logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add Request path and QueryString as property enrichers for logging.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
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
