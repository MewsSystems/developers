using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;

/// <summary>
/// Command to reset a provider's health status after manual intervention.
/// </summary>
public record ResetProviderHealthCommand(int ProviderId) : ICommand<Result>;
