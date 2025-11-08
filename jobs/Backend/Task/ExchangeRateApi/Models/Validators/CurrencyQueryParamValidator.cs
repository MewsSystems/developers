using FluentValidation;
using System.Text.RegularExpressions;

namespace ExchangeRateApi.Models.Validators
{
	public class CurrencyQueryParamValidator : AbstractValidator<string>
	{
		private static readonly Regex QueryCodesRegex = new("^[A-Za-z]{3}(?:,[A-Za-z]{3})*$", RegexOptions.Compiled);

		public CurrencyQueryParamValidator()
		{
			RuleFor(s => s)
				.NotEmpty().WithMessage("Currency codes parameter is required")
				.Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("Currency codes parameter is required")
				.MaximumLength(1000).WithMessage("Currency query parameter is too long")
				.Must(s => QueryCodesRegex.IsMatch(s))
				.WithMessage("Currency codes must be in XXX,YYY,ZZZ format with 3-letter codes");
		}
	}
}
