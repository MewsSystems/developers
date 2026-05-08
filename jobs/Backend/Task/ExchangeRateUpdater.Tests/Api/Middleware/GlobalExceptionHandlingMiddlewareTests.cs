using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Api.Middleware;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Api.Middleware;

public class GlobalExceptionHandlingMiddlewareTests
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private readonly DefaultHttpContext _httpContext;
    private readonly RequestDelegate _nextMock;

    public GlobalExceptionHandlingMiddlewareTests()
    {
        _logger = Substitute.For<ILogger<GlobalExceptionHandlingMiddleware>>();
        _environment = Substitute.For<IHostEnvironment>();
        _httpContext = new DefaultHttpContext();
        _httpContext.Response.Body = new MemoryStream();
        _nextMock = Substitute.For<RequestDelegate>();
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextDelegate()
    {
        // Arrange
        _nextMock.Invoke(Arg.Any<HttpContext>()).Returns(Task.CompletedTask);
        var middleware = new GlobalExceptionHandlingMiddleware(_nextMock, _logger, _environment);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        await _nextMock.Received(1).Invoke(Arg.Is<HttpContext>(ctx => ctx == _httpContext));
    }

    [Fact]
    public async Task InvokeAsync_ExchangeRateProviderException_ReturnsBadGateway()
    {
        // Arrange
        const string errorMessage = "Provider error";
        _nextMock
            .Invoke(Arg.Any<HttpContext>())
            .Returns(x => throw new ExchangeRateProviderException(errorMessage));
            
        var middleware = new GlobalExceptionHandlingMiddleware(_nextMock, _logger, _environment);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        var response = await GetResponseAs<ApiResponse>();
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadGateway);
        response.Should().NotBeNull();
        response.Message.Should().Be("Exchange rate provider error");
        response.Errors.Should().Contain(errorMessage);
        response.Success.Should().BeFalse();
        
        _logger.Received(1).Log(
            Arg.Is<LogLevel>(l => l == LogLevel.Error),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Is<ExchangeRateProviderException>(e => e.Message == errorMessage),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task InvokeAsync_ArgumentException_ReturnsBadRequest()
    {
        // Arrange
        const string errorMessage = "Invalid input";
        _nextMock
            .Invoke(Arg.Any<HttpContext>())
            .Returns(x => throw new ArgumentException(errorMessage));
            
        var middleware = new GlobalExceptionHandlingMiddleware(_nextMock, _logger, _environment);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        var response = await GetResponseAs<ApiResponse>();
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        response.Should().NotBeNull();
        response.Message.Should().Be("Invalid input");
        response.Errors.Should().Contain(errorMessage);
        response.Success.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task InvokeAsync_UnhandledException_ReturnsInternalServerError(bool isDevelopment)
    {
        // Arrange
        const string errorMessage = "Unexpected error";
        _environment.EnvironmentName = isDevelopment ? Environments.Development : Environments.Production;
        _nextMock
            .Invoke(Arg.Any<HttpContext>())
            .Returns(x => throw new Exception(errorMessage));
            
        var middleware = new GlobalExceptionHandlingMiddleware(_nextMock, _logger, _environment);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        var response = await GetResponseAs<ApiResponse>();
        _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        response.Should().NotBeNull();
        response.Message.Should().Be("An error occurred while processing your request");
        response.Success.Should().BeFalse();
        
        if (isDevelopment)
        {
            response.Errors.Should().Contain(e => e.Contains(errorMessage));
        }
        else
        {
            response.Errors.Should().ContainSingle()
                .Which.Should().Be("An unexpected error occurred. Please try again later.");
        }
    }

    [Fact]
    public async Task InvokeAsync_EnsuresResponseContentTypeIsJson()
    {
        // Arrange
        _nextMock
            .Invoke(Arg.Any<HttpContext>())
            .Returns(x => throw new Exception("Test error"));
            
        var middleware = new GlobalExceptionHandlingMiddleware(_nextMock, _logger, _environment);

        // Act
        await middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.ContentType.Should().Be("application/json");
    }

    private async Task<T> GetResponseAs<T>()
    {
        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var content = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(content)!;
    }
}
