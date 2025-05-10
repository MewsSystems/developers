namespace ExchangeRateUpdater.Configuration;

public class CnbApiSettings
{
    public string BaseAddress { get; set; } = default!;
    public string Endpoint { get; set; } = default!;
    public string BaseCurrency { get; set; } = default!;
}
