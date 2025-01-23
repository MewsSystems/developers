namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;

internal class RateResponse
{
    public IEnumerable<CnbRate>? Rates { get; set; }
}