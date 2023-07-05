using System.Text.Json;

namespace CnbServiceClient.Services
{
	public abstract class BaseService
	{
		private readonly HttpClient _httpClient;
        private JsonSerializerOptions jsonSerializerOptions;

        protected BaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(body, jsonSerializerOptions);
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var result = await ParseResponseAsync<T>(response);

            return result;
        }
    }
}

