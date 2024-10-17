namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClient
    {
        Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync(LanguageCode languageCode = LanguageCode.EN);
    }
}
