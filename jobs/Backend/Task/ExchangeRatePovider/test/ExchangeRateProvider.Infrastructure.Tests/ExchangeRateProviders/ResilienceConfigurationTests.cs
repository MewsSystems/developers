using ExchangeRateProvider.Infrastructure.ExchangeRateProviders;

namespace ExchangeRateProvider.Infrastructure.Tests.ExchangeRateProviders;

public class ResilienceConfigurationTests
{
    [Fact]
    public void Timeout_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(TimeSpan.FromSeconds(10), ResilienceConfiguration.Timeout);
    }

    [Fact]
    public void MaxRetryAttempts_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(3, ResilienceConfiguration.MaxRetryAttempts);
    }

    [Fact]
    public void InitialRetryDelay_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(TimeSpan.FromMilliseconds(200), ResilienceConfiguration.InitialRetryDelay);
    }

    [Fact]
    public void UseJitter_IsTrue()
    {
        // Act & Assert
        Assert.True(ResilienceConfiguration.UseJitter);
    }

    [Fact]
    public void CircuitBreakerSamplingDuration_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(TimeSpan.FromSeconds(30), ResilienceConfiguration.CircuitBreakerSamplingDuration);
    }

    [Fact]
    public void CircuitBreakerFailureRatio_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(0.5, ResilienceConfiguration.CircuitBreakerFailureRatio);
    }

    [Fact]
    public void CircuitBreakerMinimumThroughput_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(10, ResilienceConfiguration.CircuitBreakerMinimumThroughput);
    }

    [Fact]
    public void CircuitBreakerBreakDuration_HasExpectedValue()
    {
        // Act & Assert
        Assert.Equal(TimeSpan.FromSeconds(30), ResilienceConfiguration.CircuitBreakerBreakDuration);
    }

    [Fact]
    public void CircuitBreakerFailureRatio_IsWithinValidRange()
    {
        // Act & Assert
        Assert.True(ResilienceConfiguration.CircuitBreakerFailureRatio >= 0.0);
        Assert.True(ResilienceConfiguration.CircuitBreakerFailureRatio <= 1.0);
    }

    [Fact]
    public void MaxRetryAttempts_IsPositive()
    {
        // Act & Assert
        Assert.True(ResilienceConfiguration.MaxRetryAttempts > 0);
    }

    [Fact]
    public void CircuitBreakerMinimumThroughput_IsPositive()
    {
        // Act & Assert
        Assert.True(ResilienceConfiguration.CircuitBreakerMinimumThroughput > 0);
    }

    [Fact]
    public void AllTimeSpanValues_ArePositive()
    {
        // Act & Assert
        Assert.True(ResilienceConfiguration.Timeout > TimeSpan.Zero, "Timeout should be positive");
        Assert.True(ResilienceConfiguration.InitialRetryDelay > TimeSpan.Zero, "InitialRetryDelay should be positive");
        Assert.True(ResilienceConfiguration.CircuitBreakerSamplingDuration > TimeSpan.Zero, "CircuitBreakerSamplingDuration should be positive");
        Assert.True(ResilienceConfiguration.CircuitBreakerBreakDuration > TimeSpan.Zero, "CircuitBreakerBreakDuration should be positive");
    }
}