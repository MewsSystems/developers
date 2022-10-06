using Mews.CurrencyExchange.Domain.Models;
using Mews.CurrencyExchange.Providers.Abstractions;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Configuration;

namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Provider
{
    internal class CurrencyExchangeProvider : ICurrencyExchangeProvider
    {
        private readonly ICzechNationalBankExchangeClient _czechNationalBankExchangeClient;
        private readonly ConnectorSettings _connectorSettings;

        public CurrencyExchangeProvider(ICzechNationalBankExchangeClient czechNationalBankExchangeClient, ConnectorSettings connectorSettings)
        {
            _czechNationalBankExchangeClient = czechNationalBankExchangeClient;
            _connectorSettings = connectorSettings;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies)
        {
            ValidateRequiredConfigurationAndThrowOnError();

            var bankExchangeRates = await _czechNationalBankExchangeClient.FetchExchangeRates();

            ExtractValidExchangeRates(requestedCurrencies, bankExchangeRates, out List<ExchangeRate> exchangeRates);

            return exchangeRates;
        }

        private void ExtractValidExchangeRates(IEnumerable<Currency> requestedCurrencies, IEnumerable<ProviderExchangeRate> bankExchangeRates, out List<ExchangeRate> exchangeRates)
        {
            exchangeRates = new List<ExchangeRate>();
            foreach (var requestedCurrency in requestedCurrencies)
            {
                var bankExchangeRate = bankExchangeRates.FirstOrDefault(bankExchangeRate => bankExchangeRate.Currency == requestedCurrency.Code);
                if (bankExchangeRate == null)
                    continue;
                exchangeRates.Add(new ExchangeRate(new Currency(_connectorSettings.SourceCurrency), new Currency(bankExchangeRate.Currency), bankExchangeRate.Rate));
            }
        }

        private void ValidateRequiredConfigurationAndThrowOnError()
        {
            if (string.IsNullOrEmpty(_connectorSettings.SourceCurrency))
            {
                throw new Exception($"{nameof(ConnectorSettings)}.{nameof(ConnectorSettings.SourceCurrency)} is missing from configuration");
            }
        }
    }
}
