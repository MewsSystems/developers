namespace ExchangeRateUpdater.CzechNationalBank.Contracts
{
    internal sealed record DailyExchangeRate
    {
        [JsonPropertyName("amount")]
        public long Amount { get; init; }

        [JsonPropertyName("country")]
        public string Country { get; init; } = default!;

        [JsonPropertyName("currency")]
        public string Currency { get; init; } = default!;

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; init; } = default!;

        [JsonPropertyName("order")]
        public int Order { get; init; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; init; }

        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; init; }
    }
}