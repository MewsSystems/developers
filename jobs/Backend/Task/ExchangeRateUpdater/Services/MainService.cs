using System;
using System.Threading.Tasks;
using ExchangeRateTool.Interfaces;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services
{
	public class MainService
	{
		private readonly ICurrenciesProvider _currenciesProvider;
		private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IExchangeRatePrinter _exchangeRatePrinter;

        public MainService(ICurrenciesProvider currenciesProvider, IExchangeRateProvider exchangeRateProvider, IExchangeRatePrinter exchangeRatePrinter)
        {
            _currenciesProvider = currenciesProvider;
            _exchangeRateProvider = exchangeRateProvider;
            _exchangeRatePrinter = exchangeRatePrinter;
        }

        public async Task Run()
        {
            try
            {
                var currencies = _currenciesProvider.GetCurrenciesFromConfig();

                var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

                _exchangeRatePrinter.Print(exchangeRates);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
        }
    }
}

