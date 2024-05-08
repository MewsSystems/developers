using ExchangeRateUpdater.Api.HostedServices;
using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using Microsoft.FeatureManagement;
using Moq;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionMockExtensions
{
    public static IServiceCollection RemoveHostedServices(this IServiceCollection services)
    {
        var serviceDescriptor = services.Single(d => d.ImplementationType == typeof(CnbExchangeRatesUpdater));
        services.Remove(serviceDescriptor);
        return services;
    }

    public static IServiceCollection ConfigureClientsMock(this IServiceCollection services)
    {
        return services
            .ConfigureMock<ICzechNationalBankApiClient>();
    }

    public static IServiceCollection ConfigureFeatureFlagsMock(this IServiceCollection services)
    {
        return services
            .ConfigureMock<IFeatureManager>();
    }

    private static IServiceCollection ConfigureMock<T>(this IServiceCollection services) where T : class
    {
        return services
            .AddSingleton(new Mock<T>())
            .AddSingleton<Mock>(sp => sp.GetRequiredService<Mock<T>>())
            .AddScoped(sp => sp.GetRequiredService<Mock<T>>().Object);
    }
}
