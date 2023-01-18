using Entities.Records;
using ExchangeRateUpdater;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ValidationRules.FluentValidation
{
    public class ExchangeRateListRecordValidator : AbstractValidator<ExchangeRateListRecord>
    {
        public ExchangeRateListRecordValidator()
        {
            RuleForEach(x => x.ExchangeRates).SetValidator(new ExchangeRateValidator());
        }
    }
}
