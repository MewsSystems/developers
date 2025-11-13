namespace ExchangeRateUpdater.Infrastucture.Data.API.Abstractions
{
    public interface IExchangeRatesDailyAPIService
    {
        Task<HttpResponseMessage> GetExternalDataAsync();
    }
}
