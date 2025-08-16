using System.Diagnostics.CodeAnalysis;
using Mews.ERP.AppService.Features.Fetch.Builders;
using Mews.ERP.AppService.Features.Fetch.Builders.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers;
using Mews.ERP.AppService.Features.Fetch.Networking.Providers.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Repositories;
using Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Services;
using Mews.ERP.AppService.Features.Fetch.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RestSharp;

namespace Mews.ERP.AppService.Features.Fetch.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFetchFeature(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFetchService, FetchService>()
            .AddScoped<IRestRequestBuilder, RestRequestBuilder>()
            .AddScoped<ICnbExchangeRatesProvider, CnbExchangeRatesProvider>()
            .AddScoped<ICurrenciesRepository, CurrenciesRepository>()
            .AddScoped<IRestClient, RestClient>()
            .ConfigureRestRetryPolicies();
    

    private static IServiceCollection ConfigureRestRetryPolicies(this IServiceCollection serviceCollection)
    {
        var restRetryPolicy = Policy
            .HandleResult<RestResponse>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(Constants.FetchFeature.RestMaxRetryCount, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

        return serviceCollection
            .AddSingleton<IAsyncPolicy<RestResponse>>(restRetryPolicy);
    }
}