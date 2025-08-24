using Mews.ExchangeRateMonitor.Common.Domain.Results;

namespace Mews.ExchangeRateMonitor.Common.Tests.Unit;

public class ErrorTests
{
    [Fact]
    public void Create_ShouldCreateFailureError_WhenUsingFailureFactory()
    {
        // Arrange
        const string code = "Test.Failure";
        const string description = "Test failure description";

        // Act
        var error = Error.Failure(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Failure, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateUnexpectedError_WhenUsingUnexpectedFactory()
    {
        // Arrange
        const string code = "Test.Unexpected";
        const string description = "Test unexpected description";

        // Act
        var error = Error.Unexpected(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Unexpected, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateValidationError_WhenUsingValidationFactory()
    {
        // Arrange
        const string code = "Test.Validation";
        const string description = "Test validation description";

        // Act
        var error = Error.Validation(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Validation, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateConflictError_WhenUsingConflictFactory()
    {
        // Arrange
        const string code = "Test.Conflict";
        const string description = "Test conflict description";

        // Act
        var error = Error.Conflict(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Conflict, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateNotFoundError_WhenUsingNotFoundFactory()
    {
        // Arrange
        const string code = "Test.NotFound";
        const string description = "Test not found description";

        // Act
        var error = Error.NotFound(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.NotFound, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateUnauthorizedError_WhenUsingUnauthorizedFactory()
    {
        // Arrange
        const string code = "Test.Unauthorized";
        const string description = "Test unauthorized description";

        // Act
        var error = Error.Unauthorized(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Unauthorized, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateForbiddenError_WhenUsingForbiddenFactory()
    {
        // Arrange
        const string code = "Test.Forbidden";
        const string description = "Test forbidden description";

        // Act
        var error = Error.Forbidden(code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Forbidden, error.Type);
    }

    [Fact]
    public void Create_ShouldCreateCustomError_WhenUsingCustomFactory()
    {
        // Arrange
        const string code = "Test.Custom";
        const string description = "Test custom description";
        const int customType = 100;

        // Act
        var error = Error.Custom(customType, code, description);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(description, error.Description);
        Assert.Equal(ErrorType.Custom, error.Type);
        Assert.Equal(customType, error.NumericType);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenErrorsAreEqual()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code", "Test description");
        var error2 = Error.Validation("Test.Code", "Test description");

        // Act & Assert
        Assert.Equal(error1, error2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenErrorCodesAreDifferent()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code1", "Test description");
        var error2 = Error.Validation("Test.Code2", "Test description");

        // Act & Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenErrorDescriptionsAreDifferent()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code", "Test description 1");
        var error2 = Error.Validation("Test.Code", "Test description 2");

        // Act & Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenErrorTypesAreDifferent()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code", "Test description");
        var error2 = Error.NotFound("Test.Code", "Test description");

        // Act & Assert
        Assert.NotEqual(error1, error2);
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameValue_WhenErrorsAreEqual()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code", "Test description");
        var error2 = Error.Validation("Test.Code", "Test description");

        // Act
        var hashCode1 = error1.GetHashCode();
        var hashCode2 = error2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_ShouldReturnDifferentValues_WhenErrorsAreDifferent()
    {
        // Arrange
        var error1 = Error.Validation("Test.Code1", "Test description");
        var error2 = Error.Validation("Test.Code2", "Test description");

        // Act
        var hashCode1 = error1.GetHashCode();
        var hashCode2 = error2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode1, hashCode2);
    }
}
