using ExchangeRateUpdater.BL.Interfaces;
using ExchangeRateUpdater.BL.Models;
using ExchangeRateUpdater.BL.Utilities;
using ExchangeRateUpdater.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.BL.Implementations
{
    public class ExchangeRateUpdaterService : IExchangeRateUpdaterService
    {
        private readonly IDataScrapper _dataScrapper;
        private readonly ILogger<ExchangeRateUpdaterService> _logger;
        public ExchangeRateUpdaterService(IDataScrapper dataScrapper, ILogger<ExchangeRateUpdaterService> logger)
        {
            _dataScrapper = dataScrapper;
            _logger = logger;
        }
        public IEnumerable<ExchangeRate> GetExchangeRateMappedFromSource(IEnumerable<Currency> currencies, string URL)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            try
            {
                if (currencies != null)
                {
                    var dataSource = _dataScrapper.GetRatesFromWeb(URL);
                    var defaultCurrency = ConfigurationSettings.GetDefaultCurrency();

                    if (dataSource != null)
                    {
                        foreach (var currency in currencies)
                        {
                            if (currency.Code != defaultCurrency.Code)
                            {
                                var currencyDetails = dataSource.Where(a => a.Code == currency.Code).FirstOrDefault();
                                if (currencyDetails != null)
                                {
                                    ExchangeRate er = new ExchangeRate(defaultCurrency, currency, currencyDetails.Rate);
                                    rates.Add(er);
                                }
                            }

                        }
                    }
                    else
                    {
                        _logger.LogInformation($"There is no rates available for today on the CNB Website");
                    }
                    
                }
                else
                {
                    _logger.LogInformation($"There is no currency to search for");
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error found in GetExchangeRateMappedFromSource Method. Message: {ex.Message}");
            }

            return rates.AsEnumerable();
        }
    }
}
