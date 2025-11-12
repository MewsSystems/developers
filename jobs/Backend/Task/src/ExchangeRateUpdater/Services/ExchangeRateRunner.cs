using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateRunner
    {
        Task Run();
    }

    public class ExchangeRateRunner : IExchangeRateRunner
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly ILogger<ExchangeRateRunner> _logger;

        public ExchangeRateRunner(ILogger<ExchangeRateRunner> logger, IExchangeRateProvider exchangeRateProvider)
        {
            _logger = logger;
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task Run()
        {
            try
            {
                var rates = await _exchangeRateProvider.GetExchangeRates(Constants.Currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates from CNB:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not retrieve exchange rates.");
                _logger.LogError(ex.ToString());
            }
        }
    }
}
