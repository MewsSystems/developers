using ExchangeRateProviderService.CNBExchangeRateProviderService.Client;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;
using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Mappers;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService;

public static class ExchangeRateProviderDependencies
{
    public static IServiceCollection AddExchangeRateProviderDependencies(this IServiceCollection services)
    {
        services.AddConfiguration();

        services.AddTransient<IExchangeRateJsonToDtoMapper, ExchangeRateJsonToDtoMapper>();
        services.AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>();

        services.AddTransient<IValidator<CurrencyDto>, CurrencyModelValidator>();
        services.AddTransient<IValidator<ExchangeRateDto>, ExchangeRateModelValidator>();

        services.AddHttpClient<IApiClient, ApiClient>();

        return services;
    }

    private static void AddConfiguration(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("CNBExchangeRateProviderService/apisettings.json")
            .Build();

        services.Configure<ApiClientOptions>(configuration.GetRequiredSection(ApiClientOptions.SectionName));
    }
}
