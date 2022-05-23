using Microsoft.Extensions.Primitives;

namespace ExchangeRate.WebApi.Middleware
{
	/// <summary>
	/// Middleware to include Correlation Id header into the request and response
	/// </summary>
	public class CorrelationIdMiddleware
	{
		const string XCorrelationId = "X-Correlation-Id";
		private readonly RequestDelegate _next;

		public CorrelationIdMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// InvokeAsync - Correlation middleware logic
		/// </summary>
		/// <param name="context">Http context</param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext context)
		{
			bool found = context.Request.Headers.TryGetValue(XCorrelationId, out StringValues correlationIds);
			correlationIds = found ? correlationIds : Guid.NewGuid().ToString();

			if (!found)
			{
				context.Request.Headers.Add(XCorrelationId, correlationIds);
			}

			context.Response.OnStarting(state =>
			{
				HttpContext httpContext = (HttpContext)state;
				httpContext.Response.Headers.Add(XCorrelationId, correlationIds);
				return Task.CompletedTask;
			}, context);

			await _next(context);
		}
	}
}
