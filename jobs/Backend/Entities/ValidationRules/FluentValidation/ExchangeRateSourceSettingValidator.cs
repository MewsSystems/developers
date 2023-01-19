using Entities.Models;
using FluentValidation;

namespace Entities.ValidationRules.FluentValidation
{
    public class ExchangeRateSourceSettingValidator:AbstractValidator<ExchangeRateSourceSettings>
    {
        public ExchangeRateSourceSettingValidator()
        {
            RuleFor(er => er.SourceCurrency).NotEmpty();
            RuleFor(er => er.SourceCurrency).NotNull();
            RuleFor(er => er.SourceCurrency).Length(3);
            RuleFor(er => er.SourceUrl).NotEmpty();
            RuleFor(er => er.SourceUrl).NotNull();
        }
    }
}
