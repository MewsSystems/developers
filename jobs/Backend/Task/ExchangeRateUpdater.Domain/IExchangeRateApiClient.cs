namespace ExchangeRateUpdater.Domain
{
    public interface IExchangeRateApiClient
    {
        Currency TargetCurrency { get; }

        Task<IReadOnlyList<ApiExchangeRate>> GetDailyExchangeRatesAsync(LanguageCode languageCode = LanguageCode.EN);
    }
}
