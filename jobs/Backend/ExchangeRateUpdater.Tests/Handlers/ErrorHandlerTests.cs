using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Services.Handlers;
using FluentAssertions;
using FluentResults;

namespace ExchangeRateUpdater.Tests.Handlers
{
    public class ErrorHandlerTests
    {
        [Fact]
        public void Handle_CreatesFailedResultWithCorrectErrorCode()
        {
            // Arrange
            var errorCode = CnbErrorCode.NetworkError;
            var message = "Test network error";

            // Act
            var result = ErrorHandler.Handle<string>(errorCode, message);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            result.Errors[0].Message.Should().Be(message);
            result.Errors[0].Metadata.Should().ContainKey("ErrorCode");
            result.Errors[0].Metadata["ErrorCode"].Should().Be(errorCode);
        }

        [Fact]
        public void ExtractError_WithErrorCodeInMetadata_ReturnsCorrectCnbException()
        {
            // Arrange
            var expectedCode = CnbErrorCode.TimeoutError;
            var expectedMessage = "Request timed out";
            var result = Result.Fail(new Error(expectedMessage)
                .WithMetadata("ErrorCode", expectedCode));

            // Act
            var exception = ErrorHandler.ExtractError(result);

            // Assert
            exception.Should().NotBeNull();
            exception.ErrorCode.Should().Be(expectedCode);
            exception.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public void ExtractError_WithoutErrorCodeInMetadata_ReturnsUnexpectedError()
        {
            // Arrange
            var result = Result.Fail("Some error without metadata");

            // Act
            var exception = ErrorHandler.ExtractError(result);

            // Assert
            exception.ErrorCode.Should().Be(CnbErrorCode.UnexpectedError);
            exception.Message.Should().Be("Some error without metadata");
        }

        [Fact]
        public void ExtractError_WithEmptyErrors_ReturnsUnexpectedError()
        {
            // Arrange
            var result = Result.Ok();

            // Act
            var exception = ErrorHandler.ExtractError(result);

            // Assert
            exception.Should().NotBeNull();
            exception.ErrorCode.Should().Be(CnbErrorCode.UnexpectedError);
            exception.Message.Should().Be("Unknown error");
        }
    }
}
