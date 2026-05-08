namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    public interface IHandler
    {
        ExchangeRate GetExchangeRate(Currency currency);
    }
}
