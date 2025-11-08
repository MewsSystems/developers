using ExchangeRateApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateApi;

/// <summary>
/// Simple global exception filter for consistent API error responses
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
	private readonly ILogger<GlobalExceptionFilter> _logger;

	public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		var exception = context.Exception;
		var traceId = context.HttpContext.TraceIdentifier;

		_logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);

		var (statusCode, message) = exception switch
		{
			ArgumentException => (400, exception.Message),
			InvalidOperationException => (400, exception.Message),
			_ => (500, "An error occurred while processing your request")
		};

		object payload = statusCode switch
		{
			400 => new ErrorResponse { Error = message },
			_ => new { Error = message }
		};

		context.Result = new ObjectResult(payload)
		{
			StatusCode = statusCode
		};

		context.ExceptionHandled = true;
	}
}