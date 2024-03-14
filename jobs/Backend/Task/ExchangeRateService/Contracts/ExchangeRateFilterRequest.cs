namespace ExchangeRateService.Contracts;

public record ExchangeRateFilterRequest(string[]? CurrencyCode);

internal class ExchangeRateFilterRequestValidator : AbstractValidator<ExchangeRateFilterRequest>
{
    public ExchangeRateFilterRequestValidator()
    {
        RuleForEach(x => x.CurrencyCode)
            .NotNull()
            .Matches("^[A-Z]{3}$")
            .WithMessage("Only three-letter ISO 4217 code of the currency is supported.");
    }
}