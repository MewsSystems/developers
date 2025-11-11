using FluentValidation;

namespace ApplicationLayer.Queries.SystemHealth.GetRecentErrors;

public class GetRecentErrorsQueryValidator : AbstractValidator<GetRecentErrorsQuery>
{
    public GetRecentErrorsQueryValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(500)
            .WithMessage("Count cannot exceed 500.");

        RuleFor(x => x.Severity)
            .Must(s => string.IsNullOrEmpty(s) || new[] { "Error", "Warning", "Critical", "Information" }.Contains(s))
            .When(x => !string.IsNullOrEmpty(x.Severity))
            .WithMessage("Severity must be one of: Error, Warning, Critical, Information.");
    }
}
