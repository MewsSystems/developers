using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Api.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Presentation.Contracts.Requests;

public sealed class GetExchangeRateByCodeRequest
{
    /// <summary>
    /// ISO 4217 three-letter currency code (e.g. USD, EUR, GBP).
    /// </summary>
    [FromRoute(Name = "currencyCode")]
    [Required(ErrorMessage = "Currency code is required.")]
    [RegularExpression(@"^[a-zA-Z]{3}$",
        ErrorMessage = "Currency code must be exactly 3 letters.")]
    public string CurrencyCode { get; init; } = string.Empty;

    /// <summary>
    /// Optional date in yyyy-MM-dd format. Defaults to the latest available rates.
    /// </summary>
    [FromQuery]
    [DateFormat]
    public string? Date { get; init; }
}
