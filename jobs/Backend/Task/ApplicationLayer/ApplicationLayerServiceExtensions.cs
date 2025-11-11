using System.Reflection;
using ApplicationLayer.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationLayer;

/// <summary>
/// Extension methods for configuring ApplicationLayer services.
/// </summary>
public static class ApplicationLayerServiceExtensions
{
    /// <summary>
    /// Adds ApplicationLayer services to the dependency injection container.
    /// Registers MediatR, FluentValidation validators, and pipeline behaviors.
    /// </summary>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR (CQRS)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            // Register pipeline behaviors (order matters!)
            // 1. Unhandled exceptions (outermost)
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

            // 2. Logging
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // 3. Validation (validates both commands and queries)
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // 4. Performance monitoring
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            // 5. Transaction (innermost, only applies to commands)
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

        // Register FluentValidation validators for both commands and queries
        // Validators are automatically discovered from the assembly
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}

