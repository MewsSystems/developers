namespace ExchangeRateUpdater.WebApi.Services.Interfaces
{
    public interface IExchangeRatesDownloaderFromURL
    {
        Task<string> GetExchangeRatesRawTextFromURL(string url);
    }
}
