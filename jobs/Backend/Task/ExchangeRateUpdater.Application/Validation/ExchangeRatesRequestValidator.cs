using FluentValidation;

namespace ExchangeRateUpdater.Application.Validation;

public sealed class ExchangeRatesRequestValidator : AbstractValidator<string[]>
{
    public ExchangeRatesRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("No currencies were provided as input");

        RuleForEach(x => x)
            .NotEmpty()
            .WithMessage("Currency must not be empty")
            .Matches("^[A-Za-z]{3}$")
            .WithMessage("Each currency must be a 3-letter code. Example: USD");
    }
}