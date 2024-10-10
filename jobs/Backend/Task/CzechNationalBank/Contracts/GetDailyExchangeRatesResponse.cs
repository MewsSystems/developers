namespace ExchangeRateUpdater.CzechNationalBank.Contracts
{
    internal sealed record GetDailyExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<DailyExchangeRate> Rates { get; init; } = Enumerable.Empty<DailyExchangeRate>();
    }
}