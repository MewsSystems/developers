using Exchange.Application.Abstractions.Caching;
using Exchange.Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Infrastructure.Extensions.ServiceCollectionExtensions;

public static class CachingServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
    {
        services
            .AddMemoryCache()
            .AddSingleton<ICacheService, InMemoryCacheService>();
        return services;
    }
}