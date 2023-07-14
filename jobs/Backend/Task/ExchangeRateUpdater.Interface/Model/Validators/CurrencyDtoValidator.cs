using ExchangeRateUpdater.Interface.DTOs;
using FluentValidation;

namespace ExchangeRateUpdater.Interface.Model.Validators
{
    public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
    {
        public CurrencyDtoValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().Length(3);
        }
    }
}
