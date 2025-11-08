using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;

/// <summary>
/// Command to reschedule an exchange rate provider's recurring job.
/// Updates the UpdateTime and TimeZone configuration and reschedules the Hangfire job.
/// </summary>
public record RescheduleProviderCommand(
    string ProviderCode,
    string UpdateTime,
    string TimeZone) : ICommand<Result>;
