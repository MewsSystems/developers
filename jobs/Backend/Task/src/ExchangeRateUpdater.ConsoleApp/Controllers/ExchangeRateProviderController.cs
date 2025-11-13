using ExchangeRateUpdater.Application.Abstractions;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.ConsoleApp.Abstractions;

namespace ExchangeRateUpdater.ConsoleApp.Controllers
{
    public class ExchangeRateProviderController
    {
        private readonly ICommandReader _commandReader;
        private readonly IExchangeRateProviderService _exchangeRateProviderService;
        public ExchangeRateProviderController(ICommandReader commandReader, IExchangeRateProviderService exchangeRateProviderService)
        {
            _commandReader = commandReader;
            _exchangeRateProviderService = exchangeRateProviderService;
        }

        public async Task GetExchangeRates(IEnumerable<CurrencyModel> request)
        {
            var ratesModel = await _exchangeRateProviderService.GetExchangeRates(request);
            _commandReader.ShowRates(ratesModel);

        }
    }
}
