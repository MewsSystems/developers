using FluentValidation;

namespace ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;

/// <summary>
/// Validator for RescheduleProviderCommand.
/// </summary>
public class RescheduleProviderCommandValidator : AbstractValidator<RescheduleProviderCommand>
{
    public RescheduleProviderCommandValidator()
    {
        RuleFor(x => x.ProviderCode)
            .NotEmpty().WithMessage("Provider code is required.")
            .MaximumLength(10).WithMessage("Provider code must not exceed 10 characters.");

        RuleFor(x => x.UpdateTime)
            .NotEmpty().WithMessage("Update time is required.")
            .Matches(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$")
            .WithMessage("Update time must be in HH:mm format (e.g., 14:30).");

        RuleFor(x => x.TimeZone)
            .NotEmpty().WithMessage("Time zone is required.")
            .Must(BeValidTimeZone)
            .WithMessage("Time zone must be one of: CET, CEST, EET, EEST, UTC, GMT, or a valid IANA timezone ID.");
    }

    private bool BeValidTimeZone(string timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            return false;

        var commonTimeZones = new[] { "CET", "CEST", "EET", "EEST", "UTC", "GMT" };

        // Check if it's a common abbreviation
        if (commonTimeZones.Contains(timeZone.ToUpperInvariant()))
            return true;

        // Check if it starts with Europe/ (IANA format)
        if (timeZone.StartsWith("Europe/", StringComparison.OrdinalIgnoreCase))
            return true;

        // Try to convert to .NET TimeZoneInfo
        try
        {
            var tzId = ConvertTimeZoneId(timeZone);
            TimeZoneInfo.FindSystemTimeZoneById(tzId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string ConvertTimeZoneId(string timezone)
    {
        return timezone?.ToUpperInvariant() switch
        {
            "CET" => "Central European Standard Time",
            "CEST" => "Central European Standard Time",
            "EET" => "E. Europe Standard Time",
            "EEST" => "E. Europe Standard Time",
            "UTC" => "UTC",
            "GMT" => "GMT Standard Time",
            _ when timezone?.StartsWith("Europe/") == true => timezone,
            _ => "UTC"
        };
    }
}
