using ExchangeRateUpdater.Core.Common;

namespace ExchangeRateUpdater.Tests.Core;

public class MaybeTests
{
    [Fact]
    public void Maybe_WithValue_ShouldHaveValue()
    {
        var maybe = Maybe<string>.Nothing;
        var value = "test";
        maybe = value;

        Assert.True(maybe.HasValue);
        Assert.Equal(value, maybe.Value);
    }

    [Fact]
    public void Maybe_WithNull_ShouldNotHaveValue()
    {
        var maybe = Maybe<string>.Nothing;
        string? nullValue = null;
        maybe = nullValue;

        Assert.False(maybe.HasValue);
    }

    [Fact]
    public void Maybe_GetValueOrDefault_WithValue_ShouldReturnValue()
    {
        var value = "test";
        Maybe<string> maybe = value;

        var result = maybe.GetValueOrDefault("default");

        Assert.Equal(value, result);
    }

    [Fact]
    public void Maybe_GetValueOrDefault_WithoutValue_ShouldReturnDefault()
    {
        var defaultValue = "default";
        var maybe = Maybe<string>.Nothing;

        var result = maybe.GetValueOrDefault(defaultValue);

        Assert.Equal(defaultValue, result);
    }

    [Fact]
    public void Maybe_TryGetValue_WithValue_ShouldReturnTrue()
    {
        var value = "test";
        Maybe<string> maybe = value;

        var result = maybe.TryGetValue(out var retrievedValue);

        Assert.True(result);
        Assert.Equal(value, retrievedValue);
    }

    [Fact]
    public void Maybe_TryGetValue_WithoutValue_ShouldReturnFalse()
    {
        var maybe = Maybe<string>.Nothing;

        var result = maybe.TryGetValue(out var retrievedValue);

        Assert.False(result);
        Assert.Null(retrievedValue);
    }
}
