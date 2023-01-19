using Entities.Dtos;
using FluentValidation;

namespace Entities.ValidationRules.FluentValidation
{
    public class CurrencyListValidator:AbstractValidator<CurrencyListRecord>
    {
        public CurrencyListValidator()
        {
            RuleFor(x => x.Currencies).NotEmpty();
            RuleFor(x => x.Currencies).NotNull();
            RuleForEach(x => x.Currencies).SetValidator(new CurrencyValidator());
        }
    }
}
