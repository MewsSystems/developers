using FluentValidation;

namespace ApplicationLayer.Queries.Providers.GetAllProviders;

public class GetAllProvidersQueryValidator : AbstractValidator<GetAllProvidersQuery>
{
    public GetAllProvidersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters.");
    }
}
