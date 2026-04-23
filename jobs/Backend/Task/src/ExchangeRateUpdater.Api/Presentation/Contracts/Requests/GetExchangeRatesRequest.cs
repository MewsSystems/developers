using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Api.Presentation.Validators;

namespace ExchangeRateUpdater.Api.Presentation.Contracts.Requests;

public sealed class GetExchangeRatesRequest
{
    /// <summary>
    /// Optional comma-separated list of ISO 4217 three-letter currency codes to filter by.
    /// Example: USD,EUR,GBP
    /// </summary>
    [RegularExpression(@"^[a-zA-Z]{3}(,[a-zA-Z]{3})*$",
        ErrorMessage = "Currencies must be a comma-separated list of 3-letter ISO 4217 codes (e.g. USD,EUR,GBP).")]
    public string? Currencies { get; init; }

    /// <summary>
    /// Optional date in yyyy-MM-dd format. Defaults to the latest available rates.
    /// </summary>
    [DateFormat]
    public string? Date { get; init; }
}
