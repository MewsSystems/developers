namespace ExchangeRates.Api.Options;

public class CnbHttpClientOptions
{
    public string BaseUrl { get; set; } = default!;
    public int DefaultTimeoutInMilliseconds { get; set; } = 500;
}
