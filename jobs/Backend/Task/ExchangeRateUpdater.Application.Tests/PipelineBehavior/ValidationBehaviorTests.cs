using ExchangeRateUpdater.Application.PipelineBehavior;
using FluentValidation;
using MediatR;
using Moq;
using ValidationException = ExchangeRateUpdater.Application.Exceptions.ValidationException;

namespace ExchangeRateUpdater.Application.Tests.PipelineBehavior;

public class ValidationBehaviorTests
{
    private readonly Mock<RequestHandlerDelegate<string>> _next;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public ValidationBehaviorTests()
    {
        _next = new Mock<RequestHandlerDelegate<string>>();
        _next.Setup(n => n()).ReturnsAsync("Success");
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldPassThrough()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);
        var request = new TestRequest("valid");

        // Act
        var response = await behavior.Handle(request, _next.Object, _cancellationToken);

        // Assert
        Assert.Equal("Success", response);
        _next.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);
        var request = new TestRequest(""); // Invalid request

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await behavior.Handle(request, _next.Object, _cancellationToken));

        Assert.Contains("Value cannot be empty", exception.ErrorsDictionary["Value"]);
        _next.Verify(n => n(), Times.Never);
    }

    [Fact]
    public async Task Handle_NoValidators_ShouldPassThrough()
    {
        // Arrange
        var behavior = new ValidationBehavior<TestRequest, string>([]);
        var request = new TestRequest("valid");

        // Act
        var response = await behavior.Handle(request, _next.Object, _cancellationToken);

        // Assert
        Assert.Equal("Success", response);
        _next.Verify(n => n(), Times.Once);
    }

    // Helper classes for testing
    private record TestRequest(string Value);

    private class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("Value cannot be empty");
        }
    }
}


