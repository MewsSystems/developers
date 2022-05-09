using Core.Domain.Interfaces;
using Core.Infra.Interfaces;
using Core.Infra.Mappers;
using Core.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RestEase;
using RestEase.HttpClientFactory;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(RestClient.FactoryAssemblyName)]

namespace Core.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRateClient(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IExchangeRateDtoMapper, ExchangeRateDtoMapper>();
            services.AddScoped<IExchangeRateMapper, ExchangeRateMapper>();

            services.AddRestEaseClient<IExchangeRateClient>(
                baseAddress,
                null,
                (request, cancellationToken) =>
                {
                    return Task.CompletedTask;
                });

            return services;
        }
    }
}
