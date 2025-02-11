using ExchangeRateUpdater.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Application.Queries;

/// <summary>
/// Represents a query to retrieve exchange rates.
/// </summary>
public class GetExchangeRatesQuery : IRequest<ExchangeRateResponse>
{
    /// <summary>
    /// Gets or sets the date for exchange rates (optional, defaults to today).
    /// </summary>
    [FromQuery]
    [SwaggerParameter(Description = "Date for exchange rates in YYYY-MM-DD format. Defaults to today if not provided.")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the optional list of currencies.
    /// </summary>
    [FromQuery]
    [SwaggerParameter(Description = "List of currency codes in ISO 4217 codes (e.g., USD, EUR).")]
    public List<string>? Currencies { get; set; }
}
