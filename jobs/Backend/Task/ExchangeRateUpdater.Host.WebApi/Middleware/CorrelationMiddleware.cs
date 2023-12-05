
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace ExchangeRateUpdater.Host.WebApi.Middleware
{
    public class CorrelationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (LogContext.Push(new PropertyEnricher("CorrelationId", Guid.NewGuid())))
            {
                await next.Invoke(context);
            }
        }
    }
}
