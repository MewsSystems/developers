namespace Mews.ExchangeRateUpdater.Services.Infrastructure
{
    public interface IRestClient
    {
        Task<T?> Get<T>(string endpointUrl, Dictionary<string, object> parameters);

        Task<HttpResponseMessage> Get(string endpointUrl, Dictionary<string, object> parameters);

        Task<T?> ReadResponse<T>(HttpResponseMessage response);
    }
}
