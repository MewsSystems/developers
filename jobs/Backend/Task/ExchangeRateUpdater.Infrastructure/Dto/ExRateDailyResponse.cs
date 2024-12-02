namespace ExchangeRateUpdater.Infrastructure.Dto;

internal record ExRateDailyResponse(IEnumerable<ExRateDailyRest>? Rates = null)
{
	public IEnumerable<ExRateDailyRest> Rates { get; init; } = Rates ?? [];
}