using ExchangeRateUpdater;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ValidationRules.FluentValidation
{
    public class ExchangeRateValidator:AbstractValidator<ExchangeRate>
    {
        public ExchangeRateValidator()
        {
            RuleFor(c => c.SourceCurrency).NotEmpty();
            RuleFor(c => c.SourceCurrency).NotNull();
            RuleFor(c => c.SourceCurrency.Code).Length(3).WithMessage("Currency code must include 3 letters");
        }
    }
}
