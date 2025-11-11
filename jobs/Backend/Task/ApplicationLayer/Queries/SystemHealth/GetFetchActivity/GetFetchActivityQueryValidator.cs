using FluentValidation;

namespace ApplicationLayer.Queries.SystemHealth.GetFetchActivity;

public class GetFetchActivityQueryValidator : AbstractValidator<GetFetchActivityQuery>
{
    public GetFetchActivityQueryValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0.")
            .LessThanOrEqualTo(500)
            .WithMessage("Count cannot exceed 500.");

        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .When(x => x.ProviderId.HasValue)
            .WithMessage("Provider ID must be positive.");
    }
}
