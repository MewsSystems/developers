using System.Collections.Generic;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Controllers.v1.Responses;

public class GetExchangeRatesForCurrenciesResponse
{
    public IReadOnlyCollection<ExchangeRate> ExchangeRates { get; init; }
}