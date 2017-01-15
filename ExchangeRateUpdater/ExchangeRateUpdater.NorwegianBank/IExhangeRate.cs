using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.NorwegianBank
{
    interface IExhangeRate
    {
        ExchangeRate ExchangeRate { get; }
    }
}
