using FluentValidation;
using System;
using System.Linq;

namespace ExchangeRateUpdater.WebApi
{
    public class AppSettings
    {
        public string ExchangeRateProviderUrl { get; set; } = string.Empty;
        public string SourceCurrency { get; set; } = string.Empty;
        
        public void EnsureIsOk()
        {
            var validator = new AppSettingsValidator();
            var result = validator.Validate(this);
            if (result.Errors.Count == 0)
                return;

            var errors = string.Join(Environment.NewLine, result.Errors.Select(x => x.ErrorMessage));
            throw new Exception(string.Join(Environment.NewLine, errors));
        }
    }

    public class AppSettingsValidator : AbstractValidator<AppSettings>
    {
        public AppSettingsValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ExchangeRateProviderUrl).NotEmpty();
            RuleFor(x => x.SourceCurrency).NotEmpty();
        }
    }
}
