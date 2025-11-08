using ExchangeRateApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateApi.Tests;

[TestFixture]
public class GlobalExceptionFilterTests
{
	private GlobalExceptionFilter _filter = null!;
	private ILogger<GlobalExceptionFilter> _logger = null!;

	[SetUp]
	public void SetUp()
	{
		_logger = Substitute.For<ILogger<GlobalExceptionFilter>>();
		_filter = new GlobalExceptionFilter(_logger);
	}

	[Test]
	public void OnException_ArgumentException_ReturnsBadRequestErrorResponse()
	{
		var ctx = CreateExceptionContext(new ArgumentException("bad args"));
		_filter.OnException(ctx);

		Assert.Multiple(() =>
		{
			Assert.That(ctx.ExceptionHandled, Is.True);
			var result = ctx.Result as ObjectResult;
			Assert.That(result, Is.Not.Null);
			Assert.That(result!.StatusCode, Is.EqualTo(400));
			Assert.That(result.Value, Is.InstanceOf<ErrorResponse>());
			Assert.That(((ErrorResponse)result.Value!).Error, Does.Contain("bad args"));
		});

		// verify an error log happened (any error-level log)
		_logger.ReceivedWithAnyArgs().Log(default, default, default, default, default!);
	}

	[Test]
	public void OnException_InvalidOperationException_ReturnsBadRequest()
	{
		var ctx = CreateExceptionContext(new InvalidOperationException("not allowed"));
		_filter.OnException(ctx);

		var result = ctx.Result as ObjectResult;
		Assert.Multiple(() =>
		{
			Assert.That(result, Is.Not.Null);
			Assert.That(result!.StatusCode, Is.EqualTo(400));
			Assert.That(result.Value, Is.InstanceOf<ErrorResponse>());
			Assert.That(((ErrorResponse)result.Value!).Error, Does.Contain("not allowed"));
		});
	}

	[Test]
	public void OnException_GenericException_ReturnsInternalServerErrorAnonymousPayload()
	{
		var ctx = CreateExceptionContext(new Exception("boom"));
		_filter.OnException(ctx);

		var result = ctx.Result as ObjectResult;
		Assert.Multiple(() =>
		{
			Assert.That(result, Is.Not.Null);
			Assert.That(result!.StatusCode, Is.EqualTo(500));
			Assert.That(result.Value, Is.Not.InstanceOf<ErrorResponse>());
			var errorProp = result.Value!.GetType().GetProperty("Error");
			Assert.That(errorProp, Is.Not.Null);
			Assert.That(errorProp!.GetValue(result.Value) as string, Is.EqualTo("An error occurred while processing your request"));
		});
	}

	private static ExceptionContext CreateExceptionContext(Exception ex)
	{
		var httpContext = new DefaultHttpContext();
		httpContext.TraceIdentifier = Guid.NewGuid().ToString();
		var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
		return new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = ex };
	}
}
