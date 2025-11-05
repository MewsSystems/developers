using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IExchangeRateVendor, CurrencyApiExchangeRateVendor>(httpClient =>
        {
            string baseAddress =  configuration["CurrencyApi:BaseUrl"] ?? throw new ArgumentException("CurrencyApi:BaseUrl is not set in configuration");
            string apiKey =  configuration["CurrencyApi:ApiKey"] ?? throw new ArgumentException("CurrencyApi:ApiKey is not set in configuration");

            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Add("apiKey", apiKey);
        });
        
        return services;
    }
}