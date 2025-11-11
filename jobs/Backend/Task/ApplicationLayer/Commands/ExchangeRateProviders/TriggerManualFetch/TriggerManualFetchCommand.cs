using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;

/// <summary>
/// Command to manually trigger an immediate fetch of exchange rates for a specific provider.
/// </summary>
public record TriggerManualFetchCommand(int ProviderId) : ICommand<Result<string>>;
