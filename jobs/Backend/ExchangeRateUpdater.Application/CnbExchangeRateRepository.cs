using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Integration;
using System.Collections.Concurrent;

namespace ExchangeRateUpdater.Application
{
    /// <summary>
    /// Implementation of <see cref="IExchangeRateRepository"/> which stores the rates table obtained from CNB just in the memory.
    /// </summary>
    public class CnbExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ICnbApiClient _cnbApiClient;
        private ConcurrentDictionary<string, ExchangeRate> _cnbData = new ConcurrentDictionary<string, ExchangeRate>();

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cnbApiClient"></param>
        public CnbExchangeRateRepository(ICnbApiClient cnbApiClient)
        {
            _cnbApiClient = cnbApiClient;
        }

        public Task Initialize()
        {
            _cnbData.Clear();

            return
                Task.WhenAll(
                    InitBasic(),
                    InitOther());
        }

        private async Task InitBasic() => AddToDictionary(await _cnbApiClient.GetBasicRatesAsync());

        private async Task InitOther() => AddToDictionary(await _cnbApiClient.GetOtherCurrenciesRatesAsync());

        private void AddToDictionary(IEnumerable<CnbExchangeRate> data)
        {
            if (data != null)
            {
                foreach (var rate in data)
                {
                    ExchangeRate exRate = new ExchangeRate(
                        new Currency(rate.CurrencyCode),
                        new Currency("CZK"),
                        rate.Rate / rate.Count);

                    if (!_cnbData.TryAdd(rate.CurrencyCode, exRate))
                    {
                        //TODO: Log duplicit or invalid currency key.
                    }
                }
            }
        }

        public bool Any() => _cnbData.Any();

        public ExchangeRate? TryGet(string currencyCode)
        {
            if (_cnbData.TryGetValue(currencyCode, out var exchangeRate))
            {
                return exchangeRate;
            }

            return null;
        }
    }
}
