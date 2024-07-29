using ExchangeRateUpdater.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.ServiceContracts.ExchangeRate
{
    public interface IExchangeRateGetService
    {

        /// <summary>
        /// Gets all exchange available exchange rates
        /// </summary>
        /// <returns>Returns a list of the ExchangeRateResponse object</returns>
        Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates();

        /// <summary>
        /// Gets available exchange rates and filters them based on the Currency Codes specified in the parameter
        /// </summary>
        /// <param name="currencyCodes">List of Currency Codes to filter the returned results</param>
        /// <returns>Returns the intersection of exchange rates returned by the source repository 
        /// that match the Currency Codes provided in the parameter</returns>
        Task<IEnumerable<ExchangeRateResponse>> GetFilteredExchangeRates(List<string> currencyCodes);
    }
}
