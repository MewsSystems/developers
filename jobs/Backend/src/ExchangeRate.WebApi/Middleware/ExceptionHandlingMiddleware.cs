using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Framework.Exceptions;

namespace ExchangeRate.WebApi.Middleware
{
	/// <summary>
	/// Exception handling middleware
	/// </summary>
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		#region private members

		private async Task HandleExceptionAsync(HttpContext context, Exception e)
		{
			var errorMessage = PrepareGlobalErrorItem(e, out int statusCode);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			await context.Response.WriteAsync(errorMessage);
		}

		private string PrepareGlobalErrorItem(Exception e, out int statusCode)
		{
			statusCode = (int)HttpStatusCode.InternalServerError;
			string message = e.Message;
			switch (e)
			{
				case ArgumentNullException:
				case ConfigurationException:
				case EmptyResultSetException:
					_logger.LogCritical(e, message);
					statusCode = (int)HttpStatusCode.BadRequest;
					break;
				case HttpRequestException:
					_logger.LogCritical(e, message);
					statusCode = (int)HttpStatusCode.RequestTimeout;
					break;
				case ParsingException:
					_logger.LogCritical(e, message);
					break;
				case ValidationException:
					_logger.LogWarning(e, message);
					statusCode = (int)HttpStatusCode.UnprocessableEntity;
					break;
				default:
					message = "Something went wrong... ";
					_logger.LogCritical(e, message);
					break;
			}

			return JsonSerializer.Serialize(message);
		}

		#endregion
	}
}
