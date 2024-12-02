using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ExchangeRateUpdater.IntegrationTests.Common.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse?> PostAsync<TBody, TResponse>(this HttpClient httpClient, string uri, TBody body)
    {
        var serializedBody = JsonSerializer.Serialize(body);
        var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        var httpResponse = await httpClient.PostAsync(uri, content);
        httpResponse.EnsureSuccessStatusCode();
        
        var responseContent = await httpResponse.Content.ReadAsStreamAsync();
        var deserializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var deserializedResponseContent = await JsonSerializer.DeserializeAsync<TResponse?>(responseContent, deserializationOptions);
        return deserializedResponseContent;
    }
}