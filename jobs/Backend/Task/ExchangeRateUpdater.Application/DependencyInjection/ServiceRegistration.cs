using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.PipelineBehavior;
using ExchangeRateUpdater.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExchangeRateUpdater.Application.DependencyInjection;

/// <summary>
/// Provides an extension method for registering application layer dependencies in the DI container.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registers application-level services, MediatR handlers, validation behaviors, and FluentValidation validators.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register dependencies into.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the registered services.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR with pipeline behaviors
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register application services
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        return services;
    }
}
