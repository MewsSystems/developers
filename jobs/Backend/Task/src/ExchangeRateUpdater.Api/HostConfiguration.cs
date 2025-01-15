using ExchangeRateUpdater.Api.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Api
{
    [ExcludeFromCodeCoverage]
    public static class HostConfiguration
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services)
        {
            services
                .AddTransient<IApiKeyValidation, ApiKeyValidation>();

            return services
                .AddControllers().Services;
        }
    }
}
