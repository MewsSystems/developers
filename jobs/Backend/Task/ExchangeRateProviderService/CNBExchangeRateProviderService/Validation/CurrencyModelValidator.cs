using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Constants;
using FluentValidation;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;

public class CurrencyModelValidator : AbstractValidator<CurrencyDto>, IValidator<CurrencyDto>
{
    public CurrencyModelValidator()
    {
        RuleFor(currency => currency.Code).NotEmpty();
        RuleFor(currency => currency.Code).Length(Defaults.CURRENCY.CurrencyISOLength);
    }
}
