using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string baseAddress =  configuration["CurrencyApi:BaseUrl"] ?? throw new ArgumentException("CurrencyApi:BaseUrl is not set in configuration");
        string apiKey =  configuration["CurrencyApi:ApiKey"] ?? throw new ArgumentException("CurrencyApi:ApiKey is not set in configuration");
        
        services.AddHttpClient<IExchangeRateVendor, CurrencyApiExchangeRateVendor>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Add("apiKey", !string.IsNullOrWhiteSpace(apiKey) ? apiKey : throw new ArgumentException("CurrencyApi:ApiKey is not set in configuration"));
        });
        
        return services;
    }
}