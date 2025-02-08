using ExchangeRateUpdater.Contract;
using FluentValidation;
using JetBrains.Annotations;

namespace ExchangeRateUpdater.Api.Validation;

internal interface ICurrenciesSourceValidator : IValidator<List<string>>
{
}

[UsedImplicitly]
internal sealed class CurrenciesSourceValidator : AbstractValidator<List<string>>, ICurrenciesSourceValidator
{
    public CurrenciesSourceValidator()
    {
        RuleForEach(x => x)
            .NotEmpty().WithMessage(_ => "Currencies list cannot be empty.")
            .Must(code => Currency.CurrencyCodeRegex.IsMatch(code))
            .WithMessage(codes =>
                $"Currency code must be a three-letter uppercase ISO 4217 code. Received: {string.Join(',', codes)}");
    }
}