namespace ExchangeRateUpdater.Application.Configuration;

public sealed class ExchangeRateConfig
{
    public string BaseUrl { get; set; }
    
    public string DefaultCurrency { get; set; }
    
    public int Timeout { get; set; }
    
    public int RatePrecision { get; set; }
}