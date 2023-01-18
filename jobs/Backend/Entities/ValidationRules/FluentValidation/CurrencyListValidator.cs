using Entities.Dtos;
using Entities.ValidationRules.FluentValidation;
using ExchangeRateUpdater;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ValidationRules.FluentValidation
{
    public class CurrencyListValidator:AbstractValidator<CurrencyListRecord>
    {
        public CurrencyListValidator()
        {
            RuleForEach(x => x.Currencies).SetValidator(new CurrencyValidator());
        }
    }
}
