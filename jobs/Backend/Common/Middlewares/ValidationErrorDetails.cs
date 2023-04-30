using FluentValidation.Results;

namespace Common.Middlewares
{
    public class ValidationErrorDetails : ExceptionErrorDetails
    {
        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}
