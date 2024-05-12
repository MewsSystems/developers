using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;
using ExchangeRateUpdater.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ExchangeRateUpdater.ApiTests")]
namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationBootstrapper
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        AddMediatR(services);
        services.AddScoped<IExternalExchangeRateProvider, ExternalExchangeRateProvider>();
        services.AddHttpClient<ExternalExchangeRateProviderHttpClient>(client =>
        {
            var url = configuration.GetValue<string>("CNBExchangeRateApi:Url")
                ?? throw new NullReferenceException("CNBExchangeRateApi Url is null");
            client.BaseAddress = new Uri(url);
        });
        services.AddScoped<IValidator<GetExchangeRatesByCurrencyQuery>, GetExchangeRatesByCurrencyQueryValidator>();
    }

    public static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(config => config
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
