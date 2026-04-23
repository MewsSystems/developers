using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Api.Presentation.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DateFormatAttribute : ValidationAttribute
{
    private const string ExpectedFormat = "yyyy-MM-dd";

    public DateFormatAttribute()
        : base("Date must be in yyyy-MM-dd format and represent a valid date.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not string dateString || string.IsNullOrWhiteSpace(dateString))
            return true;

        return DateOnly.TryParseExact(dateString, ExpectedFormat, out _);
    }
}
