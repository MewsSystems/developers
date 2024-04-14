namespace MewsFinance.Infrastructure.Http
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl);

            return response;
        }
    }
}
