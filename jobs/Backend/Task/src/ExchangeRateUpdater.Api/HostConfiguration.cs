using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Api
{
    [ExcludeFromCodeCoverage]
    public static class HostConfiguration
    {
        public static IServiceCollection AddHostServices(this IServiceCollection services)
        {
            return services
                .AddControllers().Services;
        }
    }
}
