using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.ConsoleApp.Abstractions;

namespace ExchangeRateUpdater.ConsoleApp
{
    public class CommandReader : ICommandReader
    {
        private readonly IView _view;

        public CommandReader(IView view)
        {
            _view = view;
        }

        public void ShowRates(ExchangeRateModel ratesModel)
        {
            _view.LineFeed();
            _view.WriteLine($"Successfully retrieved {ratesModel.ExchangeRates.Count()} exchange rates:");
            foreach (var rate in ratesModel.ExchangeRates)
            {
                _view.LineFeed();
                _view.WriteLine(rate);
            }
        }
    }
}
