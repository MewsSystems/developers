using Mews.ERP.AppService.Features.Fetch.Networking.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Mews.ERP.AppService.Features.Fetch.Networking.Providers.Base;

public abstract class ExchangeRateProvider
{
    private readonly IRestClient restClient;

    protected readonly ILogger<ExchangeRateProvider> Logger;
    
    protected ExchangeRateProvider(
        IRestClient restClient,
        ILogger<ExchangeRateProvider> logger)
    {
        this.restClient = restClient;
        Logger = logger;
    }

    protected async Task<IEnumerable<ExchangeRateResponse>?> GetRates(RestRequest request)
    {
        Logger.LogInformation($"Calling external provider: {request.Resource}");
            
        var response = await restClient.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorDescription = JsonConvert.DeserializeObject(response.Content!);
            var errorMessage =
                $"Request - {request.Resource} failed. Status code: {response.StatusCode} - message: {errorDescription}";

            Logger.LogError(errorMessage);
            throw new ApplicationException(errorMessage);
        }

        if (string.IsNullOrWhiteSpace(response.Content))
        {
            return Enumerable.Empty<ExchangeRateResponse>();
        }

        return JsonConvert
            .DeserializeObject<ExchangeRateRootResponse>(response.Content)?
            .Rates!;
    }
}