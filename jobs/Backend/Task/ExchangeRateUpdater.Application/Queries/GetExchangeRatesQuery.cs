using ExchangeRateUpdater.Domain.Models;
using MediatR;
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
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the optional list of currencies.
    /// </summary>
    public List<string>? Currencies { get; set; }
}
