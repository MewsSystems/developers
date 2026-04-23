namespace ExchangeRateUpdater.Application;

using Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Transient;
            options.GenerateTypesAsInternal = true;
            options.Assemblies = [typeof(ServiceCollectionExtensions).Assembly];
            options.PipelineBehaviors = [typeof(MessageValidatorBehaviour<,>)];
        })
        .AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}