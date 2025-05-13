namespace ExchangeRateUpdater.Options;

public class CnbExchangeRateOptions
{
    public required string BaseUrl { get; set; }
    public required string CommonUrl { get; set; }
    public required string UncommonUrl { get; set; }
}
