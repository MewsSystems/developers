using ExchangeRateUpdater.src;
using ExchangeRateUpdaterModels.Models;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterConsole.src
{
    /// <summary>
    /// Provides exchange rates from the Czech National Bank API.
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        /// <summary>
        /// Instance of the Czech National Bank API client.
        /// </summary>
        private readonly CzechNationalBankAPI _api;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRateProvider"/> class.
        /// </summary>
        public ExchangeRateProvider()
        {
            _api = new CzechNationalBankAPI();
        }

        /// <summary>
        /// Retrieves exchange rates for the specified currencies from the Czech National Bank API.
        /// Filters the rates to include only those defined by the source.
        /// </summary>
        /// <param name="currencies">A collection of currencies to filter the exchange rates.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of filtered exchange rates.</returns>
        public async Task<IEnumerable<ExchangeRateModel>> GetExchangeRatesAsync(IEnumerable<CurrencyModel> currencies)
        {
            IEnumerable<ExchangeRateModel> rates = await _api.GetRatesAsync();

            var filteredRates = rates.Where(item => currencies.Any(c => c.Code == item.SourceCurrency.Code));

            Log.Information($"Filtered exchange rates to {filteredRates.Count()} items");

            return filteredRates;
        }
    }
}
