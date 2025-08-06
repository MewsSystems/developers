using ExchangeRateUpdater.Infrastructure.Providers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ExchangeRateUpdater.Infrastructure.Providers.Tests;

public class RefitLoggingHandlerTests
{
    private readonly Mock<ILogger<RefitLoggingHandler>> _mockLogger;
    private readonly RefitLoggingHandler _handler;

    public RefitLoggingHandlerTests()
    {
        _mockLogger = new Mock<ILogger<RefitLoggingHandler>>();
        _handler = new RefitLoggingHandler(_mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidLogger_ShouldCreateSuccessfully()
    {
        var handler = new RefitLoggingHandler(_mockLogger.Object);
        Assert.NotNull(handler);
    }
} 