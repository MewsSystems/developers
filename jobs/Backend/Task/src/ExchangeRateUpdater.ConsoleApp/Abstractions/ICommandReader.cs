using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.ConsoleApp.Abstractions
{
    public interface ICommandReader
    {
        void ShowRates(ExchangeRateModel rates);
    }
}
