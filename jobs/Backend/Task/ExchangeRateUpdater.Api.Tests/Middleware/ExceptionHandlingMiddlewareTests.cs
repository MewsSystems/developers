using ExchangeRateUpdater.API.Middleware;
using ExchangeRateUpdater.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using ValidationException = ExchangeRateUpdater.Application.Exceptions.ValidationException;

namespace ExchangeRateUpdater.Api.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly ExceptionHandlingMiddleware _middleware;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<RequestDelegate> _nextMock;

    public ExceptionHandlingMiddlewareTests()
    {
        _middleware = new ExceptionHandlingMiddleware();
        _httpContext = new DefaultHttpContext();
        _nextMock = new Mock<RequestDelegate>();
    }

    [Fact]
    public async Task InvokeAsync_HandlesValidationException_Returns412PreconditionFailed()
    {
        // Arrange
        var exception = new ValidationException(new Dictionary<string, string[]>
        {
            { "Date", new[] { "The date cannot be in the future." } }
        });
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status412PreconditionFailed, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesNotFoundException_Returns404NotFound()
    {
        // Arrange
        var exception = new NotFoundException("Exchange rate not found.");
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesExternalServiceException_Returns502BadGateway()
    {
        // Arrange
        var exception = new ExternalServiceException("Failed to reach external API.");
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status502BadGateway, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesParsingException_Returns500InternalServerError()
    {
        // Arrange
        var exception = new ParsingException("Invalid response format.");
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesCacheException_Returns503ServiceUnavailable()
    {
        // Arrange
        var exception = new CacheException("Cache service unavailable.");
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesGenericException_Returns500InternalServerError()
    {
        // Arrange
        var exception = new Exception("Something went wrong.");
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, _httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextMiddleware()
    {
        // Arrange
        _nextMock.Setup(n => n(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

        // Act
        await _middleware.InvokeAsync(_httpContext, _nextMock.Object);

        // Assert
        _nextMock.Verify(n => n(_httpContext), Times.Once);
        Assert.Equal(StatusCodes.Status200OK, _httpContext.Response.StatusCode);
    }
}
