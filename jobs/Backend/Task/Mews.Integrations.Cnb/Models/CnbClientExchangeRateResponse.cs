using System.Collections.Generic;

namespace Mews.Integrations.Cnb.Models;

public record CnbClientExchangeRateResponse
{
    public List<CnbClientExchangeRateResponseItem> Rates { get; init; } = [];
}