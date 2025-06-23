using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.Common;

public class RestClient : IRestClient
{
    private readonly HttpClient _httpClient;

    public RestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string uri)
    {
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await GetObjectFromResponseAsync<T>(response);
    }
    
    private static async Task<T?> GetObjectFromResponseAsync<T>(HttpResponseMessage response)
    {
        return response.Content.Headers.ContentType?.MediaType switch
        {
            "text/plain" => await ConvertTextContentToObjectAsync<T>(response),
            _ => await JsonSerializer.DeserializeAsync<T?>(await response.Content.ReadAsStreamAsync())
        };
    }
    
    private static async Task<T?> ConvertTextContentToObjectAsync<T>(HttpResponseMessage response)
    {
        var stringContent = await response.Content.ReadAsStringAsync();
        var typeConverter = TypeDescriptor.GetConverter(typeof(T));
        var convertedContent = typeConverter.ConvertFromString(stringContent);
        return (T?)convertedContent;
    }
}