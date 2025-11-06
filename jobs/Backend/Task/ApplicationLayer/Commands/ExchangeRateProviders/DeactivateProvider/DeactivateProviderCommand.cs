using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;

/// <summary>
/// Command to deactivate an exchange rate provider.
/// </summary>
public record DeactivateProviderCommand(int ProviderId) : ICommand<Result>;
