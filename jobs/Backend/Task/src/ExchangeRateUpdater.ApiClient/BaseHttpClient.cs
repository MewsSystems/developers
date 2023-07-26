using ExchangeRateUpdater.ApiClient.Common;
using ExchangeRateUpdater.ApiClient.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace ExchangeRateUpdater.ApiClient
{
    public abstract class BaseHttpClient
    {
        protected readonly ILogger _logger;
        protected readonly HttpClient _httpClient;

        protected BaseHttpClient(ILogger logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _logger = logger;
            _httpClient = httpClient;
        }

        protected async Task<TCommand> Send<TCommand, TResponse, TError>
        (HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken = default)
        where TResponse : class
        where TError : class
        where TCommand : CommandResult<TResponse, TError>, new()
        {
            var result = new TCommand();
            try
            {
                var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
                result = await HandleResponse<TCommand, TResponse, TError>(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        protected async Task<TCommand> HandleResponse<TCommand, TResponse, TError>(HttpResponseMessage httpResponseMessage)
         where TResponse : class
        where TError : class
        where TCommand : CommandResult<TResponse, TError>, new()
        {
            var result = new TCommand();
            result.StatusCode = httpResponseMessage.StatusCode;

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                result.WithPayload(JsonConvert.DeserializeObject<TResponse>(content));
                return result;
            }

            var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
            result.WithError(JsonConvert.DeserializeObject<TError>(errorContent))
                .WithErrors($"The server did not response an 20X result");

            return result;
        }

        protected virtual void HandleException(Exception ex)
        {
            ex = ex ?? throw new ArgumentNullException(nameof(ex));
            _logger.LogError(ex, $"An error happens trying to call the cnb api");
            throw new ApiCnbException(ex.Message, ex.InnerException);
        }
    }

}
