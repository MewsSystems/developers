namespace Infrastructure.Clients.Cnb
{
    public interface ICnbzClient
    {

        Task<string> GetExchangeRateAmountCsvAsync(DateTimeOffset date);

    }
}
