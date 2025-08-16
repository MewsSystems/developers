using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mews.ExchangeRate.Http.Abstractions.Dtos;

namespace Mews.ExchangeRate.Http.Abstractions
{
    public interface IExchangeRateServiceClient
    {
        /// <summary>
        /// Fetch the Central bank exchange rate fixing for the given date asynchronously .
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRateDto>> GetCurrencyExchangeRatesAsync(DateTime date);

        /// <summary>
        /// Fetch the Forex rates of other currencies for the given date asynchronously.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRateDto>> GetForeignCurrencyExchangeRatesAsync(DateTime date);
    }
}
