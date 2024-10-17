namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClient
    {
        Currency Currency { get; }

        Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync(LanguageCode languageCode = LanguageCode.EN);
    }
}
