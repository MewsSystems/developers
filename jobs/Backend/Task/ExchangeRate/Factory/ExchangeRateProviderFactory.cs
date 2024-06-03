using ExchangeRateUpdater.ExchangeRate.Exception;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank;
using Microsoft.Extensions.Logging;
using System;

namespace ExchangeRateUpdater.ExchangeRate.Factory
{
    /// <summary>
    /// Factory for creating exchange rate providers.
    /// </summary>
    /// <remarks>
    /// This factory is responsible for creating instances of exchange rate providers based on the specified currency.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExchangeRateProviderFactory"/> class.
    /// </remarks>
    /// <param name="czechNationalBankClient">The Czech National Bank client.</param>
    /// <param name="czechNationalBankExchangeRateProvider">The logger for the Czech National Bank exchange rate provider.</param>
    internal class ExchangeRateProviderFactory(ICzechNationalBankClient czechNationalBankClient, ILogger<CzechNationalBankExchangeRateProvider> czechNationalBankExchangeRateProviderLogger) : IExchangeRateProviderFactory
    {
        private readonly ICzechNationalBankClient _czechNationalBankClient = czechNationalBankClient ?? throw new ArgumentNullException(nameof(czechNationalBankClient));
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _czechNationalBankExchangeRateProviderLogger = czechNationalBankExchangeRateProviderLogger ?? throw new ArgumentNullException(nameof(czechNationalBankExchangeRateProviderLogger));

        /// <summary>
        /// Gets the appropriate exchange rate provider for the specified currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>An instance of the exchange rate provider.</returns>
        public IExchangeRateProvider GetProvider(Currency currency)
        {
            return currency.Code switch
            {
                "CZK" => new CzechNationalBankExchangeRateProvider(_czechNationalBankClient, _czechNationalBankExchangeRateProviderLogger),
                _ => throw new ExchangeRateUpdaterException($"No exchange rate provider for source currency: {currency}"),
            };
        }
    }
}
