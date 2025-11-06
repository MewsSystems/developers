using FluentValidation;

namespace ApplicationLayer.Commands.Currencies.CreateCurrency;

public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Currency code is required.")
            .Length(3)
            .WithMessage("Currency code must be exactly 3 characters (ISO 4217 standard).")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency code must contain only uppercase letters.");
    }
}
