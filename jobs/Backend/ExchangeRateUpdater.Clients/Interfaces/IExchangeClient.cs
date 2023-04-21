namespace ExchangeRateUpdater.Clients.Interfaces
{
    public interface IExchangeClient
    {
        public Task<string> GetExchangeRateTxtAsync(string currencyCode);
        public Task<string> GetFxExchangeRateTxtAsync(string currencyCode);
    }
}