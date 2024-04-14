namespace MewsFinance.Infrastructure.Http
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl);
    }
}
