using FluentValidation.Results;

namespace ExchangeRateUpdater.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = [];
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(string propertyName, string errorMessage)
        : this([new ValidationFailure { PropertyName = propertyName, ErrorMessage = errorMessage }])
    {

    }

    public Dictionary<string, string[]> Errors { get; }
}
