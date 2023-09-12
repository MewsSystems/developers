using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Providers;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers
{
    internal class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly IEnumerable<IExchangeRateProvider> _providers;
        private readonly ILogger<ExchangeRateProviderFactory> _logger;

        public ExchangeRateProviderFactory(IEnumerable<IExchangeRateProvider> providers, ILogger<ExchangeRateProviderFactory> logger)
        {
            _providers = providers;
            _logger = logger;
        }

        public IExchangeRateProvider Create(CurrencyCode targetCurrency)
        {
            _logger.LogInformation("ExchangeRateProviderFactory.Create: Creating ExchangeRateProvider for currency {currency}", targetCurrency);

            var provider = _providers.FirstOrDefault(x => x.SupportedCurrencies.Contains(targetCurrency));

            if (provider == null)
            {
                throw new ArgumentException($"Provider for currency {targetCurrency} is not supported");
            }

            return provider;
        }
    }
}
