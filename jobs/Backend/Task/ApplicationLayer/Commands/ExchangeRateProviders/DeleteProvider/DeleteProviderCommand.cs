using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;

/// <summary>
/// Command to delete an exchange rate provider.
/// Only allows deletion if the provider has no associated exchange rates.
/// </summary>
public record DeleteProviderCommand(int ProviderId, bool Force = false) : ICommand<Result>;
