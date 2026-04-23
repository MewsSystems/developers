using Exchange.Application.Abstractions.Caching;
using Exchange.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Infrastructure.Extensions.ServiceCollectionExtensions;

public static class CachingServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryCache(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.SectionName));
        
        services
            .AddMemoryCache()
            .AddSingleton<ICacheService, InMemoryCacheService>();
        return services;
    }
}