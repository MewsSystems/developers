using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateRunner
    {
        void Run();
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

        public void Run()
        {
            try
            {
                var rates = _exchangeRateProvider.GetExchangeRates(Constants.Currencies);

                _logger.LogInformation("Successfully retrieved {rates.Count()} exchange rates:", rates.Count());
                foreach (var rate in rates)
                {
                    _logger.LogInformation(rate.ToString());
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
