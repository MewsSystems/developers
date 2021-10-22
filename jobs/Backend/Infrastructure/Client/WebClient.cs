using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Client
{
    public class WebClient : IClient
    {
        private readonly ILogger<WebClient> _logger;
        private HttpClient _client;

        public WebClient(ILogger<WebClient> logger)
        {
            _logger = logger;

            InitializeClient();
        }

        public void InitializeClient()
        {
            _logger.LogInformation("Initializing the Http WebClient");

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
        }

        public async Task<string> GetAsStringAsync(string url)
        {
            _logger.LogInformation($"Fetching data from {url}");

            using var responseMessage = await _client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
                return await responseMessage.Content.ReadAsStringAsync();

            throw ErrorMessage("Failed fetching data from API");
        }

        private Exception ErrorMessage(string message)
        {
            _logger.LogError(message);
            return new ArgumentNullException(message);
        }
    }
}
