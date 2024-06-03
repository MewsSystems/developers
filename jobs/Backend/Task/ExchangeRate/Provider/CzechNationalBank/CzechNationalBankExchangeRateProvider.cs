using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Model;
using ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CzechNationalBankExchangeRateProvider"/> class with the specified dependencies.
    /// </summary>
    /// <param name="czechNationalBankClient">The Czech National Bank client.</param>
    /// <param name="logger">The logger.</param>
    internal class CzechNationalBankExchangeRateProvider(ICzechNationalBankClient czechNationalBankClient, ILogger<CzechNationalBankExchangeRateProvider> logger) : IExchangeRateProvider
    {
        private readonly ICzechNationalBankClient _czechNationalBankClient = czechNationalBankClient ?? throw new ArgumentNullException(nameof(czechNationalBankClient));
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public async Task<IEnumerable<ExchangeRateData>> GetDailyExchangeRates(DateOnly targetDate, Language language, CancellationToken cancellationToken)
        {
            // Supports only EN and CZ languages
            if (language != Language.CZ && language != Language.EN)
            {
                _logger.LogInformation($"Unsupported language {language}. Defaulting to {Language.EN}.");
                language = Language.EN;
            }

            var request = new FetchCzechNationalBankDailyExchangeRateRequest(targetDate, language);
            var response = await _czechNationalBankClient.GetDailyExchangeRates(request, cancellationToken);
            var exchangeRates = response.ExchangeRates.Select(cnbRate => new ExchangeRateData(cnbRate.Currency, cnbRate.CurrencyCode, cnbRate.Country, decimal.Divide(cnbRate.Rate, cnbRate.Amount))).ToList();
            return exchangeRates;
        }

        /// <inheritdoc/>
        public IEnumerable<Language> GetSupportedLanguages()
        {
            return [Language.CZ, Language.EN];
        }
    }
}
