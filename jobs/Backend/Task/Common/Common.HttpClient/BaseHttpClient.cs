using HttpRequestException = Common.Exceptions.HttpRequestException;

namespace Common.HttpClient;

public abstract class BaseHttpClient
{
    #region Fields

    private readonly System.Net.Http.HttpClient _httpClient;

    #endregion

    #region Constructors

    protected BaseHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    #endregion

    /// <summary>
    ///     Executes request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected async Task<string?> ExecuteHttpRequest(string? url)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(url, "Url cannot be null or empty");

            var response = await _httpClient.GetAsync(url);

            LogHttpRequest(response);

            return await GetResponse(response);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    /// <summary>
    ///     Converts content returned by the <see cref="System.Net.Http.HttpClient" /> to string.
    /// </summary>
    /// <param name="responseMessage">Response message returned by the <see cref="System.Net.Http.HttpClient" /></param>
    /// <returns>Content converted to string</returns>
    /// <exception cref="Exceptions.HttpRequestException">
    ///     Throws when status code is not considered valid. Only codes between
    ///     200 and 300 are considered as valid.
    /// </exception>
    protected virtual async Task<string> GetResponse(HttpResponseMessage responseMessage)
    {
        try
        {
            if ((int)responseMessage.StatusCode < 200 || (int)responseMessage.StatusCode >= 300)
                throw new HttpRequestException(responseMessage.StatusCode, responseMessage.Content);

            return await responseMessage.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException(ex.Message, responseMessage.StatusCode, responseMessage.Content);
        }
    }

    /// <summary>
    ///     Logs every response message received from the <see cref="System.Net.Http.HttpClient" />
    /// </summary>
    /// <param name="responseMessage">Response message returned by the <see cref="System.Net.Http.HttpClient" /></param>
    protected virtual void LogHttpRequest(HttpResponseMessage responseMessage)
    {
        Serilog.Log.Debug($"Received request with '{(int)responseMessage.StatusCode} - {responseMessage.StatusCode}' code", responseMessage);
    }

    /// <summary>
    ///     Handle every exception that might occurs during the <see cref="BaseHttpClient.ExecuteHttpRequest" />
    /// </summary>
    /// <param name="ex">Exception that occured</param>
    /// <exception cref="Exceptions.HttpRequestException">Transformed exception</exception>
    protected virtual void HandleException(Exception ex)
    {
        Serilog.Log.Error(ex, "Something went wrong with request");

        throw new HttpRequestException(ex.Message);
    }
}