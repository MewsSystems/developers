using System.Globalization;
using ExchangeRate.Domain.Providers.CzechNationalBank;
using FluentValidation;

namespace ExchangeRate.Domain.Validators;

public sealed class CzechNationalBankProviderRequestValidator : AbstractValidator<CzechNationalBankProviderRequest>
{
    public CzechNationalBankProviderRequestValidator()
    {
        RuleFor(request => request.Date)
            .Must(BeAValidDate)
            .Unless(request => request.Date == default)
            .WithMessage("Date must be a valid ISO format date (yyyy-MM-dd).");

        RuleFor(request => request.Lang)
            .IsEnumName(typeof(LanguageTypes), false)
            .Unless(request => request.Lang == default)
            .WithMessage("Language must be CZ or EN");
    }

    private bool BeAValidDate(string value)
    {
        return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}