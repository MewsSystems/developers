using Domain.Abstractions.Data;
using FluentValidation;

namespace Application.UseCases.ExchangeRates
{
    public class GetExchangeRateQueryValidator : AbstractValidator<GetExchangeRateQuery>
    {
        public GetExchangeRateQueryValidator(
            IAvailableLangauges availableLangauges,
            IAvailableCurrencies availableCurrencies)
        {
            var listOfLanguages = availableLangauges.GetLanguages();
            var listOfCurrencies = availableCurrencies.GetCurrencies().Select(x => x.Code);

            RuleFor(x => x.Language)
                .NotEmpty()
                .NotNull()
                .MaximumLength(2)
                .Must(language => listOfLanguages.Contains(language.ToUpper()))
                .WithMessage("The language is not valid");

            RuleFor(x => x.SourceCurrency)
                .NotEmpty()
                .NotNull()
                .MaximumLength(3);

            RuleFor(x => x.TargetCurrency)
                .NotEmpty()
                .NotNull()
                .MaximumLength(3);
        }
    }
}
