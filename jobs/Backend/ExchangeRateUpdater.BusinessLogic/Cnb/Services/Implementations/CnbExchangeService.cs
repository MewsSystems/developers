using ExchangeRateUpdater.BusinessLogic.Cnb.Parsers;
using ExchangeRateUpdater.BusinessLogic.Implementations;
using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.BusinessLogic.Cnb.Services.Implementations
{
    public class CnbExchangeService : IExchangeService
    {
        private readonly ILogger<CnbExchangeService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IExchangeClient _exchangeClient;

        public CnbExchangeService(IConfiguration config, IExchangeClient exchangeClient, ILogger<CnbExchangeService> logger)
        {
            _logger = logger;
            _configuration = config;
            _exchangeClient = exchangeClient;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRatesFound = new List<ExchangeRate>();
            foreach (var currency in currencies ?? new List<Currency>())
            {
                if (currency.Code == CnbConstants.DefaultCurrencyCode)
                {
                    _logger.LogInformation($"Found {CnbConstants.DefaultCurrencyCode} rate.");
                    exchangeRatesFound.Add(new ExchangeRate(currency, CnbConstants.GetDefaultCurrency(), 1));
                }
                else
                {
                    var foundExchangeRate = GetExchangeRate(currency);
                    if (foundExchangeRate != null)
                    {
                        _logger.LogInformation($"Found {currency} rate.");
                        exchangeRatesFound.Add(foundExchangeRate);
                    }
                }
            }
            return exchangeRatesFound;
        }

        private ExchangeRate GetExchangeRate(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            var separatorKey = _configuration.GetRequiredSection(CnbConstants.SettingsFieldsSeparatorKey).Value;
            var codeIndex = int.Parse(_configuration.GetRequiredSection(CnbConstants.SettingsExchangeRateCodeIndexKey).Value);
            var valueIndex = int.Parse(_configuration.GetRequiredSection(CnbConstants.SettingsExchangeRateValueIndexKey).Value);
            var unitsIndex = int.Parse(_configuration.GetRequiredSection(CnbConstants.SettingsExchangeRateUnitsIndexKey).Value);

            _logger.LogInformation($"Looking for {currency} on daily values.");
            var result = _exchangeClient.GetExchangeRateTxtAsync(currency.Code).GetAwaiter().GetResult();
            decimal? foundValue = CnbExchangeParser.ParseCnbExchangeRate(separatorKey, currency, result, unitsIndex, codeIndex, valueIndex);
            if (!foundValue.HasValue)
            {
                var fxUnitsIndex = int.Parse(_configuration.GetRequiredSection(CnbConstants.SettingsFxExchangeRateUnitsIndexKey).Value);
                var fxValueIndex = int.Parse(_configuration.GetRequiredSection(CnbConstants.SettingsFxExchangeRateValueIndexKey).Value);
                _logger.LogInformation($"{currency} not found on daily values. Looking in FxExchangeRates");
                var resultFx = _exchangeClient.GetFxExchangeRateTxtAsync(currency.Code).GetAwaiter().GetResult();
                foundValue = CnbExchangeParser.ParseCnbFxExchangeRate(separatorKey, resultFx, fxUnitsIndex, fxValueIndex);
            }

            return foundValue.HasValue ? new ExchangeRate(currency, CnbConstants.GetDefaultCurrency(), foundValue.Value) : null;
        }
    }
}
