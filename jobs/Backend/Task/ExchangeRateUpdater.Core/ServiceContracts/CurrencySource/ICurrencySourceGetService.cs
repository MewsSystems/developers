using ExchangeRateUpdater.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.ServiceContracts.CurrencySource
{
    public interface ICurrencySourceGetService
    {
        /// <summary>
        /// Gets all currency sources currently in use by the application
        /// </summary>
        /// <returns>Returns a list of the CurrencySourceResponse object including Currency Code and API URL</returns>
        Task<List<CurrencySourceResponse>> GetAllCurrencySources();
    }
}
