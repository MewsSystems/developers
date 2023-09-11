namespace Mews.ERP.AppService.Features.Fetch.Networking.Responses;

public class ExchangeRateResponse
{
    public string ValidFor { get; set; } = null!;
    
    public string Country { get; set; } = null!;
    
    public int Amount { get; set; }
    
    public string CurrencyCode { get; set; } = null!;
    
    public decimal Rate { get; set; }
}