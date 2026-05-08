using FluentValidation;

namespace ExchangeRateProvider.Api.Validators;

public interface IQuoteCurrenciesValidator : IValidator<List<string>>
{
}
public class QuoteCurrenciesValidator : AbstractValidator<List<string>>, IQuoteCurrenciesValidator
{
    public QuoteCurrenciesValidator()
    {
        RuleForEach(quoteCurrencies => quoteCurrencies)
            .Must(BeValidIso4217Code)
            .WithMessage((list, currency) => $"The quote currency '{currency}' is not a valid ISO 4217 code (must be exactly 3 letters).");
    }

    private static bool BeValidIso4217Code(string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return true;
        }

        var upperCode = currencyCode.ToUpperInvariant();
        return upperCode.Length == 3 && upperCode.All(char.IsLetter);
    }
}