using FluentValidation;

namespace ApplicationLayer.Commands.Currencies.DeleteCurrency;

public class DeleteCurrencyCommandValidator : AbstractValidator<DeleteCurrencyCommand>
{
    public DeleteCurrencyCommandValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be positive.");
    }
}
