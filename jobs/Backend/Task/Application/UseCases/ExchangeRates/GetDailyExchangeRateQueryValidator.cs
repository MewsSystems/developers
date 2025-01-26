using Domain.Abstractions.Data;
using FluentValidation;

namespace Application.UseCases.ExchangeRates;

public class GetDailyExchangeRateQueryValidator : AbstractValidator<GetDailyExchangeRateQuery>
{
    public GetDailyExchangeRateQueryValidator(
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

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .NotNull()
            .MaximumLength(3)
            .Must(currency => listOfCurrencies.Contains(currency.ToUpper()))
            .WithMessage("The currency is not valid");
    }
}
