using Entities.Records;
using FluentValidation;

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
