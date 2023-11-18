using ExchangeRateUpdater.Api.Clients;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Models;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Api.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken);
    }

    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICnbClient _cnbClient;

        private readonly Currency _sourceCurrency;

        public CnbExchangeRateProvider(
            ICnbClient cnbClient,
            IOptions<SourceConfiguration> sourceConfiguration)
        {
            _cnbClient = cnbClient ?? throw new ArgumentNullException(nameof(cnbClient));

            _sourceCurrency = new Currency(sourceConfiguration.Value?.DefaultCurrency);
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            var response = await _cnbClient.GetExchangeRatesAsync(cancellationToken);

            return response.Rates
               .Where(rate => currencies.Any(curr => curr.Code == rate.CurrencyCode))
               .Select(rate => new ExchangeRate(_sourceCurrency, new Currency(rate.CurrencyCode), Math.Round(rate.Rate, 2)));
        }
    }
}
