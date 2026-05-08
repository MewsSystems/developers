using ExchangeRateProvider.Application.Abstractions;
using ExchangeRateProvider.Application.DTOs;
using ExchangeRateProvider.Application.Handlers;
using ExchangeRateProvider.Application.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Application;

[ExcludeFromCodeCoverage(Justification = "Dependency injection configuration - no testable logic, verified through integration tests")]
public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetExchangeRatesQuery, IList<ExchangeRateDto>>, GetExchangeRatesQueryHandler>();

        return services;
    }
}
