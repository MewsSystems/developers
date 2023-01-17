using ExchangeRateUpdater;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ValidationRules.FluentValidation
{
    public class CurrencyValidator:AbstractValidator<Currency>
    {
        public CurrencyValidator()
        {
            RuleFor(c => c.Code).NotEmpty();
            RuleFor(c => c.Code).NotNull();
            RuleFor(c => c.Code).Length(3).WithMessage("Currency code must include 3 letters"); ;       
        }
    }
}
