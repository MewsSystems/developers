using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;

/// <summary>
/// Command to activate an exchange rate provider.
/// </summary>
public record ActivateProviderCommand(int ProviderId) : ICommand<Result>;
