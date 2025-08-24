using Mews.ExchangeRateMonitor.Common.Domain.Results;

namespace Mews.ExchangeRateMonitor.Common.Tests.Unit;

public class ResultImplicitConversionTests
{
    [Fact]
    public void ImplicitConversion_ShouldCreateSuccessResult_FromValue()
    {
        // Arrange
        const int value = 42;

        // Act
        Result<int> result = value;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateErrorResult_FromError()
    {
        // Arrange
        var error = Error.Validation("Test.Error", "Test error description");

        // Act
        Result<int> result = error;

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(error, result.FirstError);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateErrorResult_FromErrorList()
    {
        // Arrange
        var error1 = Error.Validation("Test.Error1", "Test error description 1");
        var error2 = Error.Validation("Test.Error2", "Test error description 2");
        var errors = new List<Error> { error1, error2 };

        // Act
        Result<int> result = errors;

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(error1, result.FirstError);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateErrorResult_FromErrorArray()
    {
        // Arrange
        var error1 = Error.Validation("Test.Error1", "Test error description 1");
        var error2 = Error.Validation("Test.Error2", "Test error description 2");
        var errors = new[] { error1, error2 };

        // Act
        Result<int> result = errors;

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(error1, result.FirstError);
        Assert.Contains(error1, result.Errors);
        Assert.Contains(error2, result.Errors);
    }
}