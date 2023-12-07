
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace ExchangeRateUpdater.Host.WebApi.Middleware
{
    /// <summary>
    /// This middleware adds CorrelationId to logging context.
    /// </summary>
    public class CorrelationMiddleware : IMiddleware
    {
        /// <summary>
        /// Add CorrelationId as a property to logging.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (LogContext.Push(new PropertyEnricher("CorrelationId", Guid.NewGuid())))
            {
                await next.Invoke(context);
            }
        }
    }
}
