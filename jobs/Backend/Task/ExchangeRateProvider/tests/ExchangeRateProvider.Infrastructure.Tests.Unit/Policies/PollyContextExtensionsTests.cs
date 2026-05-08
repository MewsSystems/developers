using ExchangeRateProvider.Infrastructure.Policies;
using Microsoft.Extensions.Logging.Testing;
using Polly;
using Shouldly;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.Policies;

public class PollyContextExtensionsTests
{
    [Fact]
    public void TryGetLogger_WithValidLogger_ReturnsTrue()
    {
        // Arrange
        var fakeLogger = new FakeLogger();
        var context = new Context("test", new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, fakeLogger }
        });

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeTrue();
        logger.ShouldBe(fakeLogger);
    }

    [Fact]
    public void TryGetLogger_WithoutLogger_ReturnsFalse()
    {
        // Arrange
        var context = new Context("test");

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeFalse();
        logger.ShouldBeNull();
    }

    [Fact]
    public void TryGetLogger_WithInvalidLoggerType_ReturnsFalse()
    {
        // Arrange
        var context = new Context("test", new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, "not a logger" }
        });

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeFalse();
        logger.ShouldBeNull();
    }

    [Fact]
    public void TryGetLogger_WithNullValue_ReturnsFalse()
    {
        // Arrange
        var context = new Context("test", new Dictionary<string, object>
        {
            { PolicyContextItems.Logger, null! }
        });

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeFalse();
        logger.ShouldBeNull();
    }

    [Fact]
    public void TryGetLogger_WithDifferentLoggerKey_ReturnsFalse()
    {
        // Arrange
        var fakeLogger = new FakeLogger();
        var context = new Context("test", new Dictionary<string, object>
        {
            { "DifferentKey", fakeLogger }
        });

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeFalse();
        logger.ShouldBeNull();
    }

    [Fact]
    public void TryGetLogger_WithEmptyContext_ReturnsFalse()
    {
        // Arrange
        var context = new Context("test", new Dictionary<string, object>());

        // Act
        var result = context.TryGetLogger(out var logger);

        // Assert
        result.ShouldBeFalse();
        logger.ShouldBeNull();
    }
}