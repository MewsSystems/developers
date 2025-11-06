using FluentValidation;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyById;

public class GetCurrencyByIdQueryValidator : AbstractValidator<GetCurrencyByIdQuery>
{
    public GetCurrencyByIdQueryValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be positive.");
    }
}
