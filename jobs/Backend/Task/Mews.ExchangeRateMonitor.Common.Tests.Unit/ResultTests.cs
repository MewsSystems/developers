using Mews.ExchangeRateMonitor.Common.Domain.Results;

namespace Mews.ExchangeRateMonitor.Common.Tests.Unit;

public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessInstance()
    {
        // Act
        var success = Result.Success;

        // Assert
        Assert.IsType<Success>(success);
    }

    [Fact]
    public void Result_ShouldImplicitlyConvertValueToResult()
    {
        // Arrange
        const string value = "test value";

        // Act
        Result<string> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsError);
        Assert.Equal(value, result.Value);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Result_ShouldImplicitlyConvertErrorToResult()
    {
        // Arrange
        var error = Error.Validation("Test.Error", "Test error description");

        // Act
        Result<string> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Equal(error, result.FirstError);
        Assert.Single(result.Errors);
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void Result_ShouldImplicitlyConvertErrorListToResult()
    {
        // Arrange
        var error1 = Error.Validation("Test.Error1", "Test error description 1");
        var error2 = Error.Validation("Test.Error2", "Test error description 2");
        var errors = new List<Error> { error1, error2 };

        // Act
        Result<string> result = errors;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Equal(error1, result.FirstError);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(error1, result.Errors);
        Assert.Contains(error2, result.Errors);
    }

    [Fact]
    public void Result_ShouldImplicitlyConvertErrorArrayToResult()
    {
        // Arrange
        var error1 = Error.Validation("Test.Error1", "Test error description 1");
        var error2 = Error.Validation("Test.Error2", "Test error description 2");
        var errors = new[] { error1, error2 };

        // Act
        Result<string> result = errors;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsError);
        Assert.Equal(error1, result.FirstError);
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(error1, result.Errors);
        Assert.Contains(error2, result.Errors);
    }

    [Fact]
    public void Result_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        // Arrange
        string? nullValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            Result<string> result = nullValue!;
        });
    }

    [Fact]
    public void Result_ShouldThrowArgumentNullException_WhenErrorListIsNull()
    {
        // Arrange
        List<Error>? nullErrors = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            Result<string> result = nullErrors!;
        });
    }

    [Fact]
    public void Result_ShouldThrowArgumentException_WhenErrorListIsEmpty()
    {
        // Arrange
        var emptyErrors = new List<Error>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            Result<string> result = emptyErrors;
        });
    }
}
