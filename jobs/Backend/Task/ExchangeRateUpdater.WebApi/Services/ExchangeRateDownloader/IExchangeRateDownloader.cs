namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;

public interface IExchangeRateDownloader
{
    Task<string> DownloadExchangeRates();
}