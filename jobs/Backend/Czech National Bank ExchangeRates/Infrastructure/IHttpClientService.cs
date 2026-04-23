namespace Czech_National_Bank_ExchangeRates.Infrastructure
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string uri, string authToken, string apiKey);
    }
}