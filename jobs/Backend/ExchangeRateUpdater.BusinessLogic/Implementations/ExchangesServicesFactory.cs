using ExchangeRateUpdater.BusinessLogic.Cnb.Services.Implementations;
using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Models;
using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Cnb.Interfaces;
using ExchangeRateUpdater.Clients.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.BusinessLogic.Implementations
{
    public class ExchangesServicesFactory : IExchangesServicesFactory
    {
        private readonly ILogger<ExchangesServicesFactory> _logger;
        private readonly ILogger<CnbExchangeService> _cnbServiceLogger;
        private readonly IConfigurationSection _cnbConfiguration;
        private readonly ICnbExchangeClient _cnbExchangeClient;

        public ExchangesServicesFactory(IConfiguration config, ICnbExchangeClient cnbExchangeClient,
            ILogger<ExchangesServicesFactory> logger, ILogger<CnbExchangeService> cnbServiceLogger)
        {
            _logger = logger;
            _cnbServiceLogger = cnbServiceLogger;
            _cnbConfiguration = config.GetSection(CnbConstants.SettingsSectionKey);
            _cnbExchangeClient = cnbExchangeClient;
        }

        public IExchangeService GetExchangeService(Currency targetCurrency)
        {
            switch (targetCurrency.Code)
            {
                case CnbConstants.DefaultCurrencyCode:
                default:
                    _logger.LogInformation($"Creating from factory {nameof(CnbExchangeService)}");
                    return new CnbExchangeService(_cnbConfiguration, _cnbExchangeClient, _cnbServiceLogger);
            }
        }
    }
}
