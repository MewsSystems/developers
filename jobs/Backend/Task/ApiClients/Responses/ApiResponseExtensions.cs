using Refit;

namespace ExchangeRateUpdater.ApiClients.Responses
{
    /// <summary>
    /// Contains extension methods for handling API responses.
    /// </summary>
    internal static class ApiResponseExtensions
    {
        /// <summary>
        /// Retrieves the endpoint URL from the provided API response.
        /// </summary>
        /// <param name="apiDailyResponse">The API response containing the exchange rates.</param>
        /// <returns>The URL of the endpoint as a string, or an empty string if the response or URL is null.</returns>
        internal static string GetEndpointUrl(this ApiResponse<GetExchangeRatesResponse> apiDailyResponse) =>
            apiDailyResponse?.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty;
    }
}