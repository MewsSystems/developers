using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace Mews.Caching
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddCustomCache(this IServiceCollection services, string name, Action<CustomCacheOptions> options)
        {
            services.AddOptions<CustomCacheOptions>(name)
                    .Configure(options)
                    .PostConfigure(options => options.Name = name);
            services.AddLogging()
                    .AddMemoryCache();
            services.TryAddSingleton<ICustomCacheFactory, CustomCacheFactory>();

            return services;
        }
    }
}
