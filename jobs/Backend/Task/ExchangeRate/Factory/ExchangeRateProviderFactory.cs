using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace ExchangeRateUpdater.ExchangeRate.Factory
{
    /// <summary>
    /// Factory for creating exchange rate providers.
    /// </summary>
    /// <remarks>
    /// This factory is responsible for creating instances of exchange rate providers based on the specified currency.
    /// </remarks>
    internal class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly ICzechNationalBankClient _czechNationalBankClient;
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _czechNationalBankExchangeRateProviderLogger;
        private readonly IOptionsMonitor<DefaultExchangeRateProviderConfig> _defaultExchangeRateProviderMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateProviderFactory"/> class.
        /// </summary>
        /// <param name="defaultExchangeRateProviderMonitor">The monitor for default exchange rate provider configuration.</param>
        /// <param name="czechNationalBankClient">The Czech National Bank client.</param>
        /// <param name="czechNationalBankExchangeRateProviderLogger">The logger for the Czech National Bank exchange rate provider.</param>
        public ExchangeRateProviderFactory(
            IOptionsMonitor<DefaultExchangeRateProviderConfig> defaultExchangeRateProviderMonitor,
            ICzechNationalBankClient czechNationalBankClient,
            ILogger<CzechNationalBankExchangeRateProvider> czechNationalBankExchangeRateProviderLogger)
        {
            _defaultExchangeRateProviderMonitor = defaultExchangeRateProviderMonitor ?? throw new ArgumentNullException(nameof(defaultExchangeRateProviderMonitor));
            _czechNationalBankClient = czechNationalBankClient ?? throw new ArgumentNullException(nameof(czechNationalBankClient));
            _czechNationalBankExchangeRateProviderLogger = czechNationalBankExchangeRateProviderLogger ?? throw new ArgumentNullException(nameof(czechNationalBankExchangeRateProviderLogger));
        }

        /// <summary>
        /// Gets the appropriate exchange rate provider for the specified currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>An instance of the exchange rate provider.</returns>
        public IExchangeRateProvider GetProvider(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);

            var defaultExchangeRateProviderConfig = _defaultExchangeRateProviderMonitor.CurrentValue;

            if (defaultExchangeRateProviderConfig == null || !defaultExchangeRateProviderConfig.ContainsKey(currency.Code))
            {
                throw new ExchangeRateUpdaterException($"No default exchange rate provider defined for source currency: {currency.Code}");
            }

            return currency.Code switch
            {
                "CZK" => defaultExchangeRateProviderConfig.GetCZKProvider() switch
                {
                    "CzechNationalBank" => new CzechNationalBankExchangeRateProvider(_czechNationalBankClient, _czechNationalBankExchangeRateProviderLogger),
                    _ => throw new ExchangeRateUpdaterException($"No default exchange rate provider defined for source currency: {currency.Code}"),
                },
                _ => throw new ExchangeRateUpdaterException($"No exchange rate provider for source currency: {currency.Code}"),
            };
        }
    }
}
