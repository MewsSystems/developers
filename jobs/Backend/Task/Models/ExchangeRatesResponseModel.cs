using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

public record ExchangeRatesResponseModel
{
    public List<ExchangeRateResponseModel> Rates { get; init; }
}