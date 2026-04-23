using ExchangeRateUpdater.Api.Domain.Common;

namespace ExchangeRateUpdater.Api.Tests.Domain.Common;

public class ResultTests
{
    public class SuccessMethod
    {
        [Fact]
        public void Success_CreatesSuccessResult()
        {
            var result = Result<int>.Success(42);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(42, result.Value);
        }
    }

    public class FailureMethod
    {
        [Fact]
        public void Failure_CreatesFailureResult()
        {
            var error = new Error("Test.Error", "Something went wrong", ErrorType.Failure);

            var result = Result<int>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal(error, result.Error);
        }

        [Fact]
        public void Failure_AccessingValue_ThrowsInvalidOperationException()
        {
            var error = new Error("Test.Error", "fail", ErrorType.Failure);
            var result = Result<int>.Failure(error);

            Assert.Throws<InvalidOperationException>(() => result.Value);
        }
    }

    public class ErrorAccess
    {
        [Fact]
        public void Error_OnSuccessResult_ThrowsInvalidOperationException()
        {
            var result = Result<int>.Success(1);

            Assert.Throws<InvalidOperationException>(() => result.Error);
        }
    }
}
