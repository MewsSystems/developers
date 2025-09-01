using ExchangeRateProvider.Domain.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateProvider.Infrastructure;

/// <summary>
/// Hosted service that registers providers during application startup.
/// This follows the standard .NET pattern for initialization tasks.
/// </summary>
public class ProviderRegistrationHostedService : IHostedService
{
    private readonly IProviderRegistrationService _providerRegistrationService;

    /// <summary>
    /// Initializes a new instance of the ProviderRegistrationHostedService class.
    /// </summary>
    /// <param name="providerRegistrationService">The provider registration service.</param>
    public ProviderRegistrationHostedService(IProviderRegistrationService providerRegistrationService)
    {
        _providerRegistrationService = providerRegistrationService ?? throw new ArgumentNullException(nameof(providerRegistrationService));
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _providerRegistrationService.RegisterProviders();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        // No cleanup needed for provider registration
        return Task.CompletedTask;
    }
}