using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;

/// <summary>
/// Command to update an exchange rate provider's configuration.
/// </summary>
public record UpdateProviderConfigurationCommand(
    int ProviderId,
    string? Name = null,
    string? Url = null,
    bool? RequiresAuthentication = null,
    string? ApiKeyVaultReference = null) : ICommand<Result>;
