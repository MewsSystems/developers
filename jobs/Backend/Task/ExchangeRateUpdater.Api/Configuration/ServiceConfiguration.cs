using ExchangeRate.Api.Clients;
using ExchangeRate.Domain.Providers;

namespace ExchangeRate.Api.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureDependencies()
            .ConfigureResiliencePolicy();
    }

    public static void ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEndpoints();
    }

    private static IServiceCollection ConfigureDependencies(this IServiceCollection services)
    {
        return services
            .RegisterExchangeRateProviders();
    }

    private static IServiceCollection RegisterExchangeRateProviders(this IServiceCollection services)
    {
        return services
            .AddKeyedTransient<IExchangeRateClient, CzechNationalBankClient>(ExchangeRateProviderType.Cnb);
    }
}