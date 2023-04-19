namespace ExchangeRateUpdater.WebApi.Services.Interfaces
{
    public interface IExchangeRatesGetter
    {
        Task<string> GetRawExchangeRates();
    }
}
