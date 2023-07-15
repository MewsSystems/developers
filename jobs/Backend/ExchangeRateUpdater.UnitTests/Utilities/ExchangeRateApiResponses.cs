using ExchangeRateUpdater.ApiClients.Responses;
using Refit;

namespace ExchangeRateUpdater.UnitTests.Utilities
{
    internal static class ExchangeRateApiResponses
    {
        internal static async Task<ApiResponse<GetExchangeRatesResponse>> GetUnsuccessfulResponse()
        {
            return new ApiResponse<GetExchangeRatesResponse>(
                new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest),
                null, null,
                await ApiException.Create(new HttpRequestMessage(), HttpMethod.Get, new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest), null, new Exception(""))
             );
        }

        internal static ApiResponse<GetExchangeRatesResponse> GetSuccessfulResponse(string currencyCode, int amount, decimal rate)
        {
            var exchangeRateApiItems = new List<ExchangeRateApiItem>
            {
                new ExchangeRateApiItem(currencyCode, amount, rate)
            };

            var apiResponseDaily = new ApiResponse<GetExchangeRatesResponse>(
                new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                new GetExchangeRatesResponse(exchangeRateApiItems),
                null, null);
            return apiResponseDaily;
        }

        internal static string GetUnsuccessfulResponseMessage() =>
            " failed with message: Response status code does not indicate success: 400 (Bad Request)..";
    }
}