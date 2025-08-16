namespace Mews.ERP.AppService.Features.Fetch.Networking.Responses;

public class ExchangeRateRootResponse
{
    public IEnumerable<ExchangeRateResponse> Rates { get; set; } = null!;
}