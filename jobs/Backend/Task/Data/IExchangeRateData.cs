namespace ExchangeRateUpdater.Data
{
    using ExchangeRateUpdater.Domain;

    public interface IExchangeRateData
    {
        BankDetails GetExchangeRateData();
    }
}