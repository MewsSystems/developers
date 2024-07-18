using Microsoft.Extensions.Options;

namespace Mews.ExchangeRate.Provider.CNB.Configuration;
internal class CnbSettingsValidator : IValidateOptions<CnbSettings>
{
    public ValidateOptionsResult Validate(string? name, CnbSettings options)
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ExratesEndpoint))
        {
            errors.Add($"{CnbSettings.Property} configuration is missing the property '{nameof(options.ExratesEndpoint)}''");
        }

        if (string.IsNullOrWhiteSpace(options.HealthcheckEndpoint))
        {
            errors.Add($"{CnbSettings.Property} configuration is missing the property '{nameof(options.HealthcheckEndpoint)}''");
        }

        return errors.Count> 0 
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
