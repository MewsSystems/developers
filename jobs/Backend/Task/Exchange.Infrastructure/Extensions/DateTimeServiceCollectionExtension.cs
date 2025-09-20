using Exchange.Infrastructure.DateTimeProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Infrastructure.Extensions;

public static class DateTimeServiceCollectionExtension
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}