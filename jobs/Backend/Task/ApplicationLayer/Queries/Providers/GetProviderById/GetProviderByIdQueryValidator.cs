using FluentValidation;

namespace ApplicationLayer.Queries.Providers.GetProviderById;

/// <summary>
/// Validator for GetProviderByIdQuery.
/// Demonstrates that queries can also be validated in the pipeline.
/// </summary>
public class GetProviderByIdQueryValidator : AbstractValidator<GetProviderByIdQuery>
{
    public GetProviderByIdQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0).WithMessage("Provider ID must be positive.");
    }
}
