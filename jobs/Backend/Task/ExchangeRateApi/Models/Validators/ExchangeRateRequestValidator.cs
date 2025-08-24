using FluentValidation;

namespace ExchangeRateApi.Models.Validators;

public class ExchangeRateRequestValidator : AbstractValidator<ExchangeRateRequest>
{
	private const int MinCodes = 1;
	private const int MaxCodes = 180; // POST limit

	public ExchangeRateRequestValidator()
	{
		RuleFor(r => r.CurrencyCodes)
			.NotNull().WithMessage("Currency codes are required")
			.Must(list => list.Count >= MinCodes && list.Count <= MaxCodes)
			.WithMessage($"Currency codes collection must contain between {MinCodes} and {MaxCodes} items");

		RuleForEach(r => r.CurrencyCodes)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Currency code cannot be empty")
			.Must(c => c.Trim().Length == 3)
			.WithMessage("Currency code must be exactly 3 characters")
			.Must(c => c.ToUpperInvariant() == c)
			.WithMessage("Currency code must be uppercase")
			.Matches("^[A-Z]{3}$").WithMessage("Currency code must match pattern 'AAA'");

		RuleFor(r => r.TargetCurrency)
			.Must(c => c == null || c.Length == 3)
			.WithMessage("Target currency must be exactly 3 characters");
	}
}