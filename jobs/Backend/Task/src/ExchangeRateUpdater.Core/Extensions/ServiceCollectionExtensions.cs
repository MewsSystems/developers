using ExchangeRateUpdater.Core.Common.Behaviors;
using ExchangeRateUpdater.Core.Features.ExchangeRates.V1.Queries;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace ExchangeRateUpdater.Core.Extensions;

/// <summary>
/// Extension methods for registering Core services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Register MediatR handlers from Core assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GetExchangeRateQuery).Assembly);

            // Add validation behavior to pipeline
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Register FluentValidation validators from Core assembly
        services.AddValidatorsFromAssemblyContaining<GetExchangeRateQuery>();

        // Register core services
        services.AddSingleton<IClock>(SystemClock.Instance);

        return services;
    }
}