using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Services.CzechCrown;
using Services.Options;

namespace Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<CzechNationalBankClientOptions>()
            .Bind(configuration.GetSection(CzechNationalBankClientOptions.CzechNationalBankClient))
            .ValidateDataAnnotations();

        services.AddHttpClient<ICzechNationalBankClient, CzechNationalBankCachedClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection(CzechNationalBankClientOptions.CzechNationalBankClient).GetValue<string>("BaseUrl")!);
        })
        .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
            [
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            ]));

        services.AddScoped<IExchangeRateProvider, CzechCrownRateProvider>();

        return services;
    }
}