using FluentValidation.Results;

namespace ExchangeRateUpdater.Application.Common.Exceptions
{
    /// <summary>
    /// Represents an exception indicating that one or more validation failures have occurred.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with a default message.
        /// </summary>
        public BadRequestException()
        : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class with validation failures.
        /// </summary>
        /// <param name="failures">The collection of validation failures.</param>
        public BadRequestException(IEnumerable<ValidationFailure> failures)
        : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        /// <summary>
        /// Gets a dictionary containing validation errors grouped by property name.
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }
    }
}
