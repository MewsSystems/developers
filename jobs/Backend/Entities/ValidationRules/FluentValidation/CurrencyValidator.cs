using Entities.Concrete;
using FluentValidation;

namespace Entities.ValidationRules.FluentValidation
{
    public class CurrencyValidator:AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(c => c.Code).NotEmpty();
            RuleFor(c => c.Code).NotNull();
            RuleFor(c => c.Code).Length(3).WithMessage("Currency code must include 3 letters");   
        }
    }
}
