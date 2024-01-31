using Mews.Shared.Temporal;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.Shared.Setup;

public static class SharedServiceCollectionExtensions
{
    /// <summary>
    /// Registers system UTC <see cref="IClock"/> service.
    /// </summary>
    public static IServiceCollection AddSystemUtcClock(this IServiceCollection services)
    {
        return services.AddTransient<IClock, SystemUtcClock>();
    }
}
