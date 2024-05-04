using ExchangeRateUpdater.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Host.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly ILogger _logger;
		private readonly RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			this._logger = logger;
			this._next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DomainException ex)
			{
				this._logger.LogError(ex, ex.Message);
				context.Response.StatusCode = (int)ex.StatusCode;
			}
			catch (Exception ex)
			{
				this._logger.LogError(ex, ex.Message);
				context.Response.StatusCode = 500;
			}
		}
	}
}
