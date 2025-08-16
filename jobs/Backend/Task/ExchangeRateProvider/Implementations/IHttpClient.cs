namespace ExchangeRateProvider.Implementations;

internal interface IHttpClient
{
    Task<HttpResponseMessage> Get(string url);
    Uri BaseAddress { set; }
    TimeSpan Timeout { set; }
    int MaxRetries { set; }
}