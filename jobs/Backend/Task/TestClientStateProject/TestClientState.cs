using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class TestClientState
{
    // Mimic the ConsoleTestApp's client structure
    class RestClient
    {
        private readonly HttpClient _httpClient;
        private string _authToken = string.Empty;

        public RestClient(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginJson = JsonSerializer.Serialize(new { email, password });
            var content = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseBody);
                _authToken = doc.RootElement.GetProperty("data").GetProperty("accessToken").GetString();

                // This is what the RestApiClient does
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);

                Console.WriteLine($"Login successful. Token set in HttpClient headers.");
                Console.WriteLine($"Auth header: {_httpClient.DefaultRequestHeaders.Authorization}");
                return true;
            }
            return false;
        }

        public async Task<bool> GetLatestRatesAsync()
        {
            Console.WriteLine($"Calling GetLatestRates...");
            Console.WriteLine($"Auth header before call: {_httpClient.DefaultRequestHeaders.Authorization}");

            var response = await _httpClient.GetAsync("/api/exchange-rates/latest/all");
            Console.WriteLine($"Response status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error response: {error}");
            }

            return response.IsSuccessStatusCode;
        }
    }

    // Mimic the ApiClientFactory
    class ClientFactory
    {
        private readonly RestClient _restClient;

        public ClientFactory()
        {
            _restClient = new RestClient("http://localhost:5188");
        }

        public RestClient GetClient()
        {
            // Always returns the same instance (like ApiClientFactory does)
            return _restClient;
        }
    }

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Testing Client State Management ===\n");

        // Create factory (like ConsoleTestApp does once)
        var factory = new ClientFactory();

        // Simulate login-all command
        Console.WriteLine("1. Simulating login-all command...");
        var client1 = factory.GetClient();
        await client1.LoginAsync("admin@example.com", "simple");

        // Simulate test-all command (gets client from factory again)
        Console.WriteLine("\n2. Simulating test-all command (using factory.GetClient again)...");
        var client2 = factory.GetClient();

        // Verify it's the same instance
        Console.WriteLine($"   Same client instance? {ReferenceEquals(client1, client2)}");

        // Try to call API
        var success = await client2.GetLatestRatesAsync();
        Console.WriteLine($"   API call successful? {success}");

        Console.WriteLine("\n=== Test Complete ===");
    }
}