/*
 * This class implements the IHttpService interface using HttpClient.
 */
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;

/// <summary>
/// Initializes a new instance of the HttpService class with the specified HttpClient dependency.
/// </summary>
/// <param name="httpClient">The HttpClient instance used to send HTTP requests.</param>
/// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
internal class HttpService(HttpClient httpClient) : IHttpService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    /// <summary>
    /// Sends an asynchronous GET request to the specified URL using the encapsulated HttpClient instance.
    /// </summary>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the response content as a string.</returns>
    public async Task<string> GetAsync(string url, CancellationToken cancellationToken)
    {
        // Send the GET request using HttpClient and ensure success status code
        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
