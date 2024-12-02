using Microsoft.FeatureManagement;

namespace Microsoft.Extensions.DependencyInjection;

public static class FeatureFlagsConfiguration
{
    public static IServiceCollection ConfigureFeatureFlags(this IServiceCollection services)
    {
        services.AddFeatureManagement();
        return services;
    }
}
