using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Providers.CNB.Models;

internal sealed record CNBExchangeRateInformation
{
    public DateOnly ValidFor { get; init; }
    public required string CurrencyCode { get; init; }
    public required decimal Rate { get; init; }
}

internal sealed record CNBExchangeRateResponse
{
    public IEnumerable<CNBExchangeRateInformation> Rates { get; init; } = [];
}

