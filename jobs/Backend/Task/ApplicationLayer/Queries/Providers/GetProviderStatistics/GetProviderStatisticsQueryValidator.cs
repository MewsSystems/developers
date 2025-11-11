using FluentValidation;

namespace ApplicationLayer.Queries.Providers.GetProviderStatistics;

public class GetProviderStatisticsQueryValidator : AbstractValidator<GetProviderStatisticsQuery>
{
    public GetProviderStatisticsQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .GreaterThan(0)
            .WithMessage("Provider ID must be positive.");
    }
}
