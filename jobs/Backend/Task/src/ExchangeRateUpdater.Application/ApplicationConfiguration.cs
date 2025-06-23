using ExchangeRateUpdater.Application.Behaviours;
using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates.ProviderStrategies;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ExchangeRateUpdater.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services
                .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddProviderStrategies();

            return services;
        }

        private static IServiceCollection AddProviderStrategies(this IServiceCollection services)
        {
            var providerStrategies = Assembly
                .GetAssembly(typeof(IExchangeRateProviderStrategy))!
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IExchangeRateProviderStrategy).IsAssignableFrom(x));

            foreach (var brandStrategy in providerStrategies)
            {
                services.AddScoped(typeof(IExchangeRateProviderStrategy), brandStrategy);
            }

            return services;
        }
    }
}
