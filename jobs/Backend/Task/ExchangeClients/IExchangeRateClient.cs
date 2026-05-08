namespace ExchangeRateUpdater.ExchangeClients
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IExchangeRateClient
    {
        Task<string> GetDailyRatesAsync(CancellationToken cancellationToken);
    }
}
