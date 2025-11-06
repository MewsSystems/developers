using FluentValidation;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyByCode;

public class GetCurrencyByCodeQueryValidator : AbstractValidator<GetCurrencyByCodeQuery>
{
    public GetCurrencyByCodeQueryValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Currency code is required.")
            .Length(3)
            .WithMessage("Currency code must be exactly 3 characters.")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency code must be 3 uppercase letters (ISO 4217 format).");
    }
}
