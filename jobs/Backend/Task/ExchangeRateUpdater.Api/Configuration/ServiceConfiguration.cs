using ExchangeRate.Api.Clients;
using ExchangeRate.Domain.Providers;
using ExchangeRate.Domain.Validators;
using FluentValidation;

namespace ExchangeRate.Api.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureDependencies()
            .ConfigureResiliencePolicy()
            .ConfigureErrorHandling();
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
        app.ConfigureExceptionMiddleware();
    }

    private static IServiceCollection ConfigureDependencies(this IServiceCollection services)
    {
        return services
            .RegisterExchangeRateProviders()
            .RegisterValidators();
    }

    private static IServiceCollection RegisterExchangeRateProviders(this IServiceCollection services)
    {
        return services
            .AddKeyedTransient<IExchangeRateClient, CzechNationalBankClient>(ExchangeRateProviderType.Cnb);
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<CzechNationalBankProviderRequestValidator>();
    }
}