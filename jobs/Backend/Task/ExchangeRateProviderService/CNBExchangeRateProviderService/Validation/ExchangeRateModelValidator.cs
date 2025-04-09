using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using FluentValidation;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;

public class ExchangeRateModelValidator : AbstractValidator<ExchangeRateDto>, IValidator<ExchangeRateDto>
{
    public ExchangeRateModelValidator(IValidator<CurrencyDto> currencyModelValidator)
    {
        RuleFor(exchangeRate => exchangeRate.BaseCurrency).SetValidator(currencyModelValidator);
        RuleFor(exchangeRate => exchangeRate.TargetCurrency).SetValidator(currencyModelValidator);
    }
}
