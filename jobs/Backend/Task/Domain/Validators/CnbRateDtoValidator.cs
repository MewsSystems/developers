using ExchangeRateUpdater.Domain.DTOs;
using FluentValidation;

namespace ExchangeRateUpdater.Domain.Validators;

public class CnbRateDtoValidator : AbstractValidator<CnbRateDto>
{
    public CnbRateDtoValidator()
    {
        RuleFor(dto => dto.ExchangeRateDtos)
            .NotNull().WithMessage("No Cnb exchange rate data found");

        RuleForEach(dto => dto.ExchangeRateDtos)
            .SetValidator(new CnbExchangeRateValidator());
    }
}