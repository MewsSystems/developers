using Mews.ExchangeRateProvider.Application.Abstractions;
using Mews.ExchangeRateProvider.Application.Repos;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRateProvider.Application.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRateRepository, RateRepository>();
            return services;
        }
    }
}
